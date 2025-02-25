using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinSimpleIDriver.Editor
{
    public partial class FormDesign : Form
    {
        public enum ResizeDirection
        {
            None, Right, Bottom
        }

        private List<ElementDataApp> appElements = new List<ElementDataApp>();

        private bool isDragging = false;
        private bool isResizing = false;
        private ResizeDirection resizeDirection = ResizeDirection.None;
        private Point offset;
        private Control selectedControl = null;

        private uint elementID = 0; // абсолютный идентификатор

        private const int ResizeMargin = 6; // Отступ в пикселях для изменения размеров

        public FormDesign()
        {
            InitializeComponent();
        }

        private void FormDesign_Load(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemAddControlLabel_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.Label);
        }

        private void ToolStripMenuItemAddControlInput_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.InputBox);
        }

        private void ToolStripMenuItemAddControlOutput_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.OutputBox);
        }

        private void ToolStripMenuItemAddControlPicture_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.PictureBox);
        }

        private void ToolStripMenuItemAddControlRectangle_Click(object sender, EventArgs e)
        {
            AddElement(eElementType.Rectangle);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFromJson();
        }

        private void saveJsonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveToJson();
        }

        private void ToolStripMenuItemCommandCopy_Click(object sender, EventArgs e)
        {
            if (selectedControl == null) return; // Проверяем, есть ли выбранный элемент

            // Находим соответствующий элемент в списке
            var originalElement = appElements.FirstOrDefault(el => el.Control == selectedControl);
            if (originalElement == null) return;

            // Увеличиваем идентификатор
            elementID++;

            // Создаем копию элемента
            var newElement = new ElementDataApp
            {
                ElementType = originalElement.ElementType,
                Control = CreateControl(originalElement.ElementType)
            };

            // Проверяем, был ли создан Control
            if (newElement.Control == null) return;

            // Присваиваем уникальное имя элементу
            newElement.Control.Name = $"{originalElement.ElementType}{elementID}";

            // Копируем основные свойства
            newElement.Control.Left = originalElement.Control.Left + 10; // Смещаем копию
            newElement.Control.Top = originalElement.Control.Top + 10;
            newElement.Control.Width = originalElement.Control.Width;
            newElement.Control.Height = originalElement.Control.Height;
            newElement.Control.Text = originalElement.Control.Text;

            if (newElement.Control is PictureBox pic && originalElement.Control is PictureBox originalPic)
            {
                pic.Image = originalPic.Image; // Копируем изображение
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                newElement.ImagePath = originalElement.ImagePath;
            }

            // Привязываем события для перемещения и изменения размеров
            AttachControlEvents(newElement.Control);

            // Добавляем элемент в список
            appElements.Add(newElement);

            // Добавляем Control на форму
            this.Controls.Add(newElement.Control);

            // Устанавливаем новый элемент как выбранный
            selectedControl = newElement.Control;
        }


        private void ToolStripMenuItemCommandDelete_Click(object sender, EventArgs e)
        {
            if (selectedControl == null) return;

            var elementToDelete = appElements.FirstOrDefault(el => el.Control == selectedControl);
            if (elementToDelete == null) return;

            // ✅ Вызов метода, который триггерит событие и закрывает окно
            elementToDelete.Delete();

            // ✅ Удаляем элемент из списка и формы
            appElements.Remove(elementToDelete);
            this.Controls.Remove(selectedControl);
            selectedControl.Dispose();
            selectedControl = null;
        }


        // ===================================================================================

        // Общая функция для добавления элементов
        private void AddElement(eElementType type)
        {
            // Увеличиваем идентификатор
            elementID++;

            // Создаем новый элемент данных
            var newElement = new ElementDataApp
            {
                ElementType = type,
                Control = CreateControl(type)
            };

            // Проверяем, был ли создан Control
            if (newElement.Control == null) return;

            // Присваиваем уникальное имя элементу
            newElement.Control.Name = $"{type}{elementID}";

            // Устанавливаем координаты элемента на форме
            newElement.Control.Left = 100;
            newElement.Control.Top = 100;

            // Привязываем события для перемещения и изменения размеров
            AttachControlEvents(newElement.Control);

            // Добавляем элемент в список
            appElements.Add(newElement);

            // Добавляем Control на форму
            this.Controls.Add(newElement.Control);
        }


        // Функция для создания Control на основе eElementType
        private Control CreateControl(eElementType type)
        {
            var controlMap = ElementDataApp.GetClassFromType();

            if (!controlMap.ContainsKey(type))
                return null;

            switch (controlMap[type])
            {
                case eUsedFormClass.Label:
                    return new Label { Text = "Label", AutoSize = true };

                case eUsedFormClass.TextBox:
                    return new TextBox();

                case eUsedFormClass.Button:
                    return new Button { Text = "Button" };

                case eUsedFormClass.PictureBox:
                    return new PictureBox { BorderStyle = BorderStyle.FixedSingle, Size = new Size(100, 100) };

                //case eUsedFormClass.ImageList:
                //    return new ImageList();

                case eUsedFormClass.ProgressBar:
                    return new ProgressBar();

                default:
                    return null;
            }
        }

        // Привязывает обработчики событий для перемещения элементов
        private void AttachControlEvents(Control control)
        {
            control.MouseDown += Control_MouseDown;
            control.MouseMove += Control_MouseMove;
            control.MouseUp += Control_MouseUp;
            control.MouseDoubleClick += Control_MouseDoubleClick; // Добавляем двойной клик
        }

        // =============================================================================

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                resizeDirection = GetResizeDirection(ctrl, e.Location);

                if (resizeDirection != ResizeDirection.None)
                {
                    isResizing = true;
                    selectedControl = ctrl;
                    offset = e.Location; // Устанавливаем начальную точку
                }
                else
                {
                    isDragging = true;
                    selectedControl = ctrl;
                    offset = e.Location;
                }

                // Обновляем статусную строку
                UpdateStatus(selectedControl);
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Control ctrl)
            {
                ctrl.Cursor = GetResizeCursor(GetResizeDirection(ctrl, e.Location));
            }

            if (isDragging && selectedControl != null)
            {
                selectedControl.Left = e.X + selectedControl.Left - offset.X;
                selectedControl.Top = e.Y + selectedControl.Top - offset.Y;
            }
            else if (isResizing && selectedControl != null)
            {
                int dx = e.X - offset.X;
                int dy = e.Y - offset.Y;

                if (resizeDirection == ResizeDirection.Right)
                {
                    selectedControl.Width = Math.Max(20, selectedControl.Width + dx);
                }
                else if (resizeDirection == ResizeDirection.Bottom)
                {
                    selectedControl.Height = Math.Max(20, selectedControl.Height + dy);
                }

                offset = new Point(e.X, e.Y); // Обновляем offset для плавности
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizing = false;

            // Если кнопка мыши все еще нажата – не сбрасываем selectedControl
            if (Control.MouseButtons != MouseButtons.None)
            {
                return;
            }

            if (sender is Control ctrl && ctrl == selectedControl)
            {
                selectedControl = ctrl;
            }
        }

        private void Control_MouseDoubleClick(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                // Ищем соответствующий элемент в списке
                var element = appElements.FirstOrDefault(el => el.Control == ctrl);
                if (element != null)
                {
                    // Открываем форму свойств и передаем текущий элемент
                    FormDesignProp propForm = new FormDesignProp(element);
                    propForm.Show(this);
                }
            }
        }

        // =================================================================================

        private ResizeDirection GetResizeDirection(Control ctrl, Point mousePosition)
        {
            bool right = mousePosition.X >= ctrl.Width - ResizeMargin;
            bool bottom = mousePosition.Y >= ctrl.Height - ResizeMargin;

            if (right && bottom) return ResizeDirection.None; // Не даем менять размер в обоих направлениях одновременно
            if (right) return ResizeDirection.Right;
            if (bottom) return ResizeDirection.Bottom;

            return ResizeDirection.None;
        }

        private Cursor GetResizeCursor(ResizeDirection direction)
        {
            switch (direction)
            {
                case ResizeDirection.Right:
                    return Cursors.SizeWE;
                case ResizeDirection.Bottom:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Обновляет статусную строку с информацией о выбранном элементе.
        /// </summary>
        /// <param name="ctrl">Выбранный элемент управления.</param>
        private void UpdateStatus(Control ctrl)
        {
            if (ctrl == null)
            {
                toolStripStatusLabelType.Text = "Не выбрано";
                toolStripStatusLabelTitle.Text = "";
                return;
            }

            // Получаем соответствие eElementType -> eUsedFormClass
            var typeMap = ElementDataApp.GetClassFromType();
            var element = appElements.FirstOrDefault(el => el.Control == ctrl);

            if (element != null && typeMap.ContainsKey(element.ElementType))
            {
                toolStripStatusLabelType.Text = typeMap[element.ElementType].ToString(); // Отображаем eUsedFormClass
                toolStripStatusLabelTitle.Text = ctrl.Name; // Отображаем имя элемента
            }
            else
            {
                toolStripStatusLabelType.Text = "Неизвестный элемент";
                toolStripStatusLabelTitle.Text = ctrl.Name;
            }
        }

        private void ToolStripMenuItemZindexBack_Click(object sender, EventArgs e)
        {
            if (selectedControl == null) return; // Проверяем, есть ли выбранный элемент

            // Перемещаем элемент позади всех
            this.Controls.SetChildIndex(selectedControl, this.Controls.Count - 1);
        }

        private void ToolStripMenuItemZindexFront_Click(object sender, EventArgs e)
        {
            if (selectedControl == null) return; // Проверяем, есть ли выбранный элемент

            // Перемещаем элемент впереди всех
            this.Controls.SetChildIndex(selectedControl, 0);
        }

        // ==============================================================

        private void TreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDesignTree frm = new FormDesignTree(this);
            frm.Show(this);
        }

        public List<ElementDataApp> GetAppElements()
        {
            return appElements;
        }

        // =========================================================================

        private void SaveToJson()
        {
            List<ElementDataJson> jsonElements = appElements
                .Select(ElementConverter.ConvertToJson)
                .ToList();

            var groupedElements = jsonElements
                .GroupBy(e => e.Page ?? "Без страницы")
                .Select(pageGroup => new
                {
                    Title = pageGroup.Key,
                    Templates = pageGroup.GroupBy(e => e.Template ?? "Без шаблона")
                        .Select(templateGroup => new
                        {
                            Title = templateGroup.Key,
                            Groups = templateGroup.GroupBy(e => e.Group == 0 ? "Без группы" : e.Group.ToString())
                                .Select(group => new
                                {
                                    Title = group.Key,
                                    Elements = group.Select(el => new
                                    {
                                        el.Name,
                                        el.ElementType,
                                        el.X,
                                        el.Y,
                                        el.Relative,
                                        el.Height,
                                        el.Width,
                                        el.Text, // ✅ Сохранение текста
                                el.Size,
                                        el.Color,
                                        el.ZIndex,
                                        el.ImagePath,
                                        el.ImagesName,
                                        el.Command,
                                        el.Visible,
                                        el.Min,
                                        el.Max,
                                        el.Value,
                                        el.ListName,
                                        el.ToolTip,
                                        el.TagTitle
                                    }).ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            var jsonData = new { Pages = groupedElements };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(jsonData, jsonSettings));
                MessageBox.Show("Конфигурация сохранена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SaveToJson2()
        {
            // Преобразуем List<ElementDataApp> в List<ElementDataJson>
            List<ElementDataJson> jsonElements = appElements
                .Select(ElementConverter.ConvertToJson)
                .ToList();

            // Группируем элементы по структуре дерева
            var groupedElements = jsonElements
                .GroupBy(e => e.Page ?? "Без страницы")
                .Select(pageGroup => new
                {
                    Title = pageGroup.Key,
                    Templates = pageGroup.GroupBy(e => e.Template ?? "Без шаблона")
                        .Select(templateGroup => new
                        {
                            Title = templateGroup.Key,
                            Groups = templateGroup.GroupBy(e => e.Group == 0 ? "Без группы" : e.Group.ToString())
                                .Select(group => new
                                {
                                    Title = group.Key,
                                    Elements = group.ToList() // Здесь остаются только ElementDataJson
                        })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            // Оборачиваем в объект с ключом "Pages"
            var jsonData = new { Pages = groupedElements };

            // Опции сериализации
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            // Сохраняем JSON в файл
            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(jsonData, jsonSettings));
                MessageBox.Show("Конфигурация сохранена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveToJson1()
        {
            // Преобразуем List<ElementDataApp> в List<ElementDataJson>
            List<ElementDataJson> jsonElements = appElements
                .Select(ElementConverter.ConvertToJson)
                .ToList();

            // Группируем элементы по структуре дерева
            var groupedElements = jsonElements
                .GroupBy(e => e.Page ?? "Без страницы")
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(e => e.Template ?? "Без шаблона")
                          .ToDictionary(
                              t => t.Key,
                              t => t.GroupBy(e => e.Group == 0 ? "Без группы" : e.Group.ToString())
                                    .ToDictionary(
                                        gr => gr.Key,
                                        gr => gr.ToList()
                                    )
                          )
                );

            // Опции сериализации
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            // Сохраняем JSON в файл
            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(groupedElements, jsonSettings));
                MessageBox.Show("Конфигурация сохранена успешно!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        // ======================================

        /// <summary>
        /// Загружает данные из config.json и добавляет элементы на форму.
        /// </summary>
        private void LoadFromJson()
        {
            if (!File.Exists("config.json"))
            {
                MessageBox.Show("Файл конфигурации не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string jsonContent = File.ReadAllText("config.json");
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

                if (jsonObject != null && jsonObject.ContainsKey("Pages"))
                {
                    var pages = JsonConvert.DeserializeObject<List<dynamic>>(jsonObject["Pages"].ToString());

                    foreach (var page in pages)
                    {
                        string pageTitle = page.Title;
                        var templates = page.Templates ?? new List<dynamic>();

                        foreach (var template in templates)
                        {
                            string templateTitle = template.Title;
                            var groups = template.Groups ?? new List<dynamic>();

                            foreach (var group in groups)
                            {
                                string groupTitle = group.Title;
                                var elements = group.Elements ?? new List<dynamic>();

                                foreach (var el in elements)
                                {
                                    ElementDataJson jsonData = new ElementDataJson
                                    {
                                        Name = el.Name ?? $"Element{elementID++}",
                                        ElementType = el.ElementType ?? "Label",
                                        X = el.X ?? 0,
                                        Y = el.Y ?? 0,
                                        Relative = el.Relative ?? false,
                                        Width = el.Width ?? 100,
                                        Height = el.Height ?? 30,
                                        Text = el.Text ?? string.Empty, // ✅ Проверка на null
                                        Size = el.Size ?? 12.0f,
                                        Color = el.Color ?? "Black", // ✅ Проверка на null
                                        ZIndex = el.ZIndex ?? 0,
                                        ImagePath = el.ImagePath ?? string.Empty
                                    };

                                    ElementDataApp newElement = ElementConverter.ConvertToApp(jsonData, CreateControl);
                                    if (newElement.Control != null)
                                    {
                                        AttachControlEvents(newElement.Control); // Добавляем обработчики событий
                                        appElements.Add(newElement);
                                        this.Controls.Add(newElement.Control);
                                    }
                                }
                            }
                        }
                    }

                    MessageBox.Show("Конфигурация загружена успешно!", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







    }


}
