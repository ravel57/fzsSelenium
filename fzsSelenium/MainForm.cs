using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using System.Globalization;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

namespace fzsSelenium
{
    public partial class MainForm : Form
    {
        private string region;
        private string tofk;
        private string role;
        private string gender;
        private string releaseBasis = "приказ";
        private bool firstCertRelease = true;
        private DataTable personDataTable;
        private static ChromeDriver driver;

        public static SettingForm.ValueRow[] constructorList;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void mainForm_Load(object sender, EventArgs e)
        {
            Task<bool> task = WaitForSettinngsDone();
            constructorList = JsonSerializer.Deserialize<SettingForm.ValueRow[]>(File.ReadAllText("settings.json"));
            await task.ContinueWith(_ => { startFzs_button.Enabled = true; },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task<bool> WaitForSettinngsDone()
        {
            while (!(
                       (cityMejReg_radioButton.Checked | cityMos_radioButton.Checked | cityMO_radioButton.Checked)
                       & (roleFL_radioButton.Checked | roleUL_radioButton.Checked | roleUL2_radioButton.Checked)
                       & ((!String.IsNullOrEmpty(openFileDialog.FileName) &
                           (genderM_radioButton.Checked | genderF_radioButton.Checked)) | !firstCertRelease)
                   ))
            {
                await Task.Delay(100);
            }

            return true;
        }

        private async void selectFile_button_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Word Documents |*.docx;*.doc";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName_label.Text = openFileDialog.FileName;
                personDataTable = new DataTable();
                WordprocessingDocument wordDoc;
                bool usedTmpFile = false;
                try
                {
                    wordDoc = WordprocessingDocument.Open(openFileDialog.FileName, false);
                }
                catch (IOException)
                {
                    File.Copy(openFileDialog.FileName, openFileDialog.FileName += ".tmp");
                    wordDoc = WordprocessingDocument.Open(openFileDialog.FileName, false);
                    usedTmpFile = true;
                }

                Table table = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().First();
                IEnumerable<TableRow> rows = table.Elements<TableRow>();
                personDataTable.Columns.Add("Поле");
                personDataTable.Columns.Add("Значение");
                foreach (TableRow row in rows)
                {
                    if (row.InnerText != "")
                    {
                        personDataTable.Rows.Add();
                        TableCell[] cells = row.Descendants<TableCell>().ToArray();
                        for (int i = 0; i < cells.Length; i++)
                        {
                            if (cells[i].InnerText != "")
                            {
                                personDataTable.Rows[personDataTable.Rows.Count - 1][i] = cells[i].InnerText.Trim();
                            }
                        }
                    }
                }

                fileView_dataGridView.DataSource = personDataTable;
                wordDoc.Close();
                if (usedTmpFile) File.Delete(openFileDialog.FileName);
            }
        }

        private void city1_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            region = "г. Москва";
            tofk = "9500";
        }

        private void city2_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            region = "г. Москва";
            tofk = "7300";
        }

        private void city3_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            region = "Московская область";
            tofk = "4800";
        }

        private void role1_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            role = "Сертификат должностного лица";
        }

        private void role2_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            role = "Сертификат юридического лица";
        }

        private void role3_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            role = "Сертификат юридического лица без ФИО";
        }

        private void genderM_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            gender = "мужской";
        }

        private void genderF_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            gender = "женский";
        }

        private void certTime1_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            firstCertRelease = true;
            startFzs_button.Enabled = false;
            mainForm_Load(sender, e);
            genderM_radioButton.Enabled = true;
            genderF_radioButton.Enabled = true;
        }

        private void certTime2_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            firstCertRelease = false;
            genderM_radioButton.Enabled = false;
            genderF_radioButton.Enabled = false;
        }

        private void powerOfAttorneyReleaseBasis_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            releaseBasis = "приказа";
        }

        private void customReleaseBasis_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            releaseBasis = customReleaseBasis_textBox.Text;
            if (customReleaseBasis_radioButton.Checked)
                customReleaseBasis_textBox.Focus();
        }

        private void customReleaseBasis_textBox_TextChanged(object sender, EventArgs e)
        {
            customReleaseBasis_radioButton.Checked = true;
            releaseBasis = customReleaseBasis_textBox.Text;
        }

        private void startFzs_button_Click(object sender, EventArgs e)
        {
            if (tofk == "9500" & !firstCertRelease)
                if (MessageBox.Show("Доверенность отвезена в казначейство?", "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
                    return;
            PersonalData personData = new PersonalData(
                (DataTable)fileView_dataGridView.DataSource,
                role.Contains("юридического лица"),
                releaseBasis == "приказа",
                powerOfAttorneyHasNumber_checkBox.Checked,
                firstCertRelease
            );
            if (personData.IsNull)
                return;
            if (personData.person.Position.Length > 140)
            {
                MessageBox.Show("Длительность должности превышает 140 симолов");
                return;
            }

            var options = new ChromeOptions();
            options.BinaryLocation = @"C:\Users\petr\AppData\Local\Chromium\Application\chrome.exe";
            options.AddExtension(@"C:\Users\petr\Downloads\iifchhfnnmpdbibifmljnfjhpififfog_main.crx");
            var driver = new ChromeDriver(@"C:\Users\petr\dev\RiderProjects\fzsSelenium\fzsSelenium\", options);
            driver.Manage().Window.Maximize();
            IJavaScriptExecutor js = driver;

            if (firstCertRelease)
                driver.Navigate().GoToUrl("https://fzs.roskazna.ru/public/requests/new");
            else
            {
                driver.Navigate().GoToUrl("https://lk-fzs.roskazna.ru/private/requests/new");
                while (Process.GetProcessesByName("iexplore").ToArray()
                           .Count(p => p.MainWindowTitle.Contains("WebDriver - Internet Explorer")) > 0)
                {
                    Thread.Sleep(250);
                }
            }

            Thread.Sleep(3000);
            driver.FindElements(By.TagName("label")).First(el => el.Text.Equals("Ключевой носитель")).Click();
            driver.FindElements(By.CssSelector(".btn.btn-b")).First(el => el.Text.Equals("ДАЛЕЕ")).Click();

            // 1st page
            // Город
            driver.FindElement(By.CssSelector(".select2-selection.select2-selection--single")).Click();
            driver.FindElements(By.ClassName("select2-results__option"))
                .First(city => city.Text.Equals(region)).Click();

            if (firstCertRelease)
            {
                driver.FindElements(By.TagName("label")).First(checkBox => checkBox.Text.Equals("Организация")).Click();
                Thread.Sleep(500);
                // ОГРН
                driver.FindElement(By.Id("r-ogrn")).SendKeys(personData.organisation.Ogrn);
                // Ожидание ввода капчи
                do
                {
                    driver.FindElement(By.Id("userCaptchaInput")).Clear();
                    driver.FindElement(By.Id("userCaptchaInput")).Click();
                    do
                    {
                        Thread.Sleep(400);
                    } while (driver.FindElement(By.Id("userCaptchaInput")).GetAttribute("value").Length < 5);

                    Thread.Sleep(100);
                    driver.FindElements(By.CssSelector(".btn.btn-b"))
                        .First(btn => btn.GetAttribute("onclick").Equals("submitCreateRequestFormWithCaptcha()"))
                        .Click();
                    Thread.Sleep(1000);
                } while (driver.FindElements(By.Id("userCaptchaInput")).Count > 0);
                //} while (driver.FindElements(By.Id("request-block")).Count == 0);
            }
            else
            {
                // ТОФК
                driver.FindElement(By.Id("Tofk")).SendKeys(tofk);
                driver.FindElement(By.Id("tofk-picker-btn")).Click();
                do
                {
                    Thread.Sleep(3000);
                } while (driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send"))
                             .Where(btn => btn.Text.Equals("ВЫБРАТЬ")).ToArray().Length == 0
                        );

                driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send"))
                    .First(btn => btn.Text.Equals("ВЫБРАТЬ")).Click();
                driver.FindElements(By.CssSelector(".btn.btn-b"))
                    .First(btn => btn.GetAttribute("value").Equals("Далее")).Click();
                do
                {
                    Thread.Sleep(1000);
                } while (driver.FindElements(By.Id("request-block")).Count == 0);
            }

            // ТОФК page
            if (firstCertRelease)
            {
                if (driver.FindElements(By.Id("CertificateRequestDestination_TOFK")).Count > 0)
                {
                    driver.FindElements(By.TagName("label")).First(el => el.Text.Equals("ТОФК")).Click();
                    //Thread.Sleep(100);
                }

                driver.FindElement(By.Id("Tofk")).SendKeys(tofk);
                driver.FindElements(By.CssSelector(".search-area-btn.ui-modal-open")).First(el => el.Text.Equals(""))
                    .Click();
                do
                {
                    //MessageBox.Show(driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send")).Where(btn => btn.Text.Equals("ВЫБРАТЬ")).ToArray().Length.ToString());
                    Thread.Sleep(3000);
                } while (driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send"))
                             .Where(btn => btn.Text.Equals("ВЫБРАТЬ")).ToArray().Length == 0
                        );

                driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send"))
                    .First(btn => btn.Text.Equals("ВЫБРАТЬ")).Click();
                Thread.Sleep(1500);
                driver.FindElements(By.CssSelector(".btn.btn-b"))
                    .First(btn => btn.GetAttribute("onclick").Equals("submitPrimaryStep3Form()")).Click();
            }

            // 2nd page
            Thread.Sleep(1500);
            //driver.FindElements(By.TagName("label")).First(checkBox => checkBox.Text.Equals(role)).Click();
            driver.FindElements(By.TagName("label")).FirstOrDefault(el => el.Text == role)?.Click();
            Thread.Sleep(1500);
            driver.FindElements(By.CssSelector(".btn.btn-b.btn-sm.pen.ui-modal-open.add-documents-link"))
                .First(el => el.Text.ToLower().Equals("ВНЕСТИ СВЕДЕНИЯ".ToLower())).Click();
            // 2nd page 1st menu
            Thread.Sleep(5000);
            if (firstCertRelease)
            {
                driver.FindElement(By.Id("Series")).SendKeys(personData.passport.Series);
                driver.FindElement(By.Id("Number")).SendKeys(personData.passport.Number);
                driver.FindElement(By.Id("DateOfIssue")).SendKeys(personData.passport.DateOfIssue);
                driver.FindElement(By.Id("CodeDivision")).SendKeys(personData.passport.CodeDivision);
                driver.FindElement(By.Id("DateOfBirth")).SendKeys(personData.person.DateOfBirth);
                driver.FindElement(By.Id("PlaceOfBirth")).SendKeys(personData.person.PlaceOfBirth);
                driver.FindElement(By.CssSelector(".ui-radiobutton.styled-radio"))
                    .FindElements(By.TagName("label")).First(checkBox => checkBox.Text.Equals(gender)).Click();
            }

            driver.FindElement(By.CssSelector(".btn.btn-lg.btn-b.pointer")).Click();
            // 2nd page 1st menu completed

            // 2nd page 2nd menu
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.pen.ui-modal-open.add-request-link")).Click();
            Thread.Sleep(3000);
            if (firstCertRelease)
            {
                driver.FindElement(By.Id("LastName")).SendKeys(personData.person.LastName);
                driver.FindElement(By.Id("FirstName")).SendKeys(personData.person.FirstName);
                driver.FindElement(By.Id("Surname")).SendKeys(personData.person.Surname);
                try
                {
                    driver.FindElement(By.Id("INN")).SendKeys(personData.person.Inn.Replace(" ", ""));
                }
                catch
                {
                }

                driver.FindElement(By.Id("SNILS")).SendKeys(personData.person.Snils);
                Clipboard.SetText(personData.person.Email);
                driver.FindElement(By.Id("Mail")).SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Insert);
                driver.FindElement(By.Id("Position")).SendKeys(personData.person.Position);
                if (role == "Сертификат юридического лица")
                {
                    if (region == "г. Москва")
                        driver.FindElement(By.Id("Locality")).SendKeys("г. Москва");
                    try
                    {
                        // driver.FindElement(By.Id("Address")).SendKeys(personData.organisation.Address);
                        driver.FindElement(By.Id("INNPersonal")).SendKeys(personData.person.Inn);
                    }
                    catch
                    {
                    }
                }
            }

            Thread.Sleep(1000);
            driver.FindElement(By.Id("SelectCsType-button")).Click();
            Thread.Sleep(1000);
            driver.FindElements(By.CssSelector(".ui-menu-item")).First(el => el.Text.Equals("KC2")).Click();
            driver.FindElement(By.CssSelector(".ui-radiobutton.styled-radio"))
                .FindElements(By.TagName("label"))
                .First(el => el.GetAttribute("for").Equals("ExportPrivateKey_True"))
                .Click();
            Thread.Sleep(1500);
            driver.FindElement(By.Id("generate-request-btn")).Click();
            // 2nd page 2nd menu completed
            // wait criptoPRO generate certificate	
            js.ExecuteScript("document.title = 'WaitCryptoPro-fzsSelenium'");
            // driver.FindElement(By.Id("modal_crtReq_container")).SendKeys(OpenQA.Selenium.Keys.End);
            do
            {
                Thread.Sleep(1000);
            } while (Process.GetProcessesByName("chrome").ToArray()
                         .Count(p => p.MainWindowTitle.Contains("WaitCryptoPro-fzsSelenium")) > 0);
            //do { Thread.Sleep(1000); }
            //while (
            //	driver.FindElements(By.Id("generate-request-btn")).Count > 0
            //);


            // save id and link
            Thread.Sleep(2500);
            string path = @".\saves\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string requestCode = new StringBuilder()
                .Append(driver.FindElement(By.Id("request-number")).Text)
                .Append("\n")
                .Append(driver.FindElements(By.TagName("a")).First(checkBox => checkBox.Text.Equals("ссылке"))
                    .GetAttribute("href"))
                .Append($"\nОГРН: {personData.organisation.Ogrn}\nИНН: {personData.organisation.Inn}")
                .ToString();
            string now = DateTime.Now.ToString(new CultureInfo("ru-RU"));
            string name;
            if (firstCertRelease)
                name = personData.person.LastName + " " +
                       personData.person.FirstName; // + " " + personData.person.surname;
            else
            {
                name = driver.FindElement(By.Id("request-fio")).Text;
                name = name.Remove(name.IndexOf(' ', name.IndexOf(' ') + 1));
            }

            string fileName = new StringBuilder()
                .Append(path)
                .Append(name)
                .Append(" ")
                .Append(role.Substring(role.IndexOf(' ') + 1, 1).ToUpper().Replace('Д', 'Ф'))
                .Append("Л ")
                .Append(now.Replace(':', '-').Remove(now.Length - 4, 3))
                .Append(".txt")
                .ToString();
            File.WriteAllText(fileName, requestCode);


            // 2nd page 3rd menu
            try
            {
                Thread.Sleep(3000);
                driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.add.ui-modal-open.add-application-link")).Click();
                driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.add.ui-modal-open.add-application-link")).Click();
            }
            catch
            {
                // ignored
            }

            Thread.Sleep(5000);
            driver.FindElement(By.Id("DocumentPowerOfAttorneyCompany_DocumentName")).SendKeys(releaseBasis);
            driver.FindElement(By.Id("DocumentPowerOfAttorneyCompany_DocumentDate"))
                .SendKeys(personData.person.PrikazDate);
            driver.FindElement(By.Id("DocumentPowerOfAttorneyCompany_DocumentNumber"))
                .SendKeys(personData.person.PrikazNumber);
            //if(driver.FindElement(By.Id("PositionOwner")).Text == "")
            //	driver.FindElement(By.Id("PositionOwner")).SendKeys(personData.person.position);
            if (!firstCertRelease && !driver.FindElement(By.Id("FullNameOwner")).GetAttribute("value")
                    .Contains(personData.person.LastName))
            {
                // driver.FindElement(By.Id("PositionManager")).SendKeys(personData.organisation.MenagerName);
                // string menagerName = personData.organisation.MenagerName;
                // menagerName = new StringBuilder()
                //     .Append(menagerName.Substring(menagerName.IndexOf(' ') + 1, 1))
                //     .Append(". ")
                //     .Append(menagerName.Substring(menagerName.IndexOf(' ', menagerName.IndexOf(' ') + 1) + 1, 1))
                //     .Append(". ")
                //     .Append(menagerName.Substring(0, menagerName.IndexOf(' ')))
                //     .ToString();
                // driver.FindElement(By.Id("FullNameManager")).SendKeys(menagerName);
            }

            driver.FindElement(By.Id("ui-upload-input-DocumentPowerOfAttorneyCompany")).Click();

            // 2nd page 3rd print page
            if (firstCertRelease)
            {
                Thread.Sleep(2000);
                do
                {
                    Thread.Sleep(500);
                } while (driver.FindElements(By.CssSelector(".btn.btn-g.ui-modal-close")).Count <= 13);

                //if (!firstCertRelease) {
                driver.FindElements(By.CssSelector(".btn.btn-b")).First(el => el.Text.Equals("ПЕЧАТЬ")).Click();
                js.ExecuteScript("document.title = 'WaitPrint-fzsSelenium'");
                Clipboard.SetText("Заявление " + name);
                //}
            }

            do
            {
                Thread.Sleep(5000);
            } while (Process.GetProcessesByName("iexplore").ToArray()
                         .Count(p => p.MainWindowTitle.Contains("WaitPrint-fzsSelenium")) > 0);

            //after print
            do
            {
                Thread.Sleep(500);
            } while (driver.FindElements(By.Id("DocumentPowerOfAttorneyCompany_DocumentDate")).Count == 0);

            try
            {
                if (!string.IsNullOrEmpty(personData.person.PrikazDate))
                {
                    driver.FindElement(By.Id("DocumentPowerOfAttorneyCompany_DocumentDate"))
                        .SendKeys(personData.person.PrikazDate + OpenQA.Selenium.Keys.Enter);
                }
            }
            catch
            {
                // ignored
            }
        }


        private void Form_Closing(object sender, CancelEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }

            File.WriteAllText("settings.json", JsonSerializer.Serialize(constructorList));
        }

        private void certTime3_radioButton_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            new SettingForm().Show();
        }
    }


    public class PersonalData
    {
        public readonly bool IsNull;

        public class Person
        {
            public string LastName; //Фамилия
            public string FirstName; //Имя
            public string Surname; //Отчество
            public string DateOfBirth; //Дата рождения
            public string PlaceOfBirth; //Место рождения
            public string Inn; //ИНН
            public string Snils; //СНИЛС
            public string Email; //E-mail
            public string Position; //Должность сотрудника (в соответствии с штатным расписанием
            public string PrikazDate; //Дата приказа
            public string PrikazNumber; //Номер приказа
        }

        public class Organisation
        {
            public string Ogrn; //огрн
            public string Inn; //инн
            // public string MenagerName; //фио руководителя
            // public string Address; //адрес
        }

        public class Passport
        {
            public string Series; //Серия
            public string Number; //Номер
            public string DateOfIssue; //Дата выдачи
            public string CodeDivision; //Код подразделения
        }

        public readonly Person person = new();
        public readonly Organisation organisation = new();
        public readonly Passport passport = new();

        enum PersonDataGroup
        {
            person,
            organisation,
            passport
        }

        public PersonalData(DataTable dataTable, bool isULrole, bool isBassedOnDoverenost,
            bool isPowerOfAttorneyHasNumber, bool isFirstCertRelease)
        {
            PersonDataGroup header = new PersonDataGroup();
            foreach (DataRow row in dataTable.Rows)
            {
                string str = row.ItemArray[0].ToString().ToLower();
                string value = row.ItemArray[1].ToString();
                switch (str)
                {
                    case "информация о сотруднике":
                        header = PersonDataGroup.person;
                        break;
                    case "данные об организации":
                        header = PersonDataGroup.organisation;
                        break;
                    case "паспортные данные (заполняется строго по паспорту)":
                        header = PersonDataGroup.passport;
                        break;
                    default:
                        switch (header)
                        {
                            case PersonDataGroup.person:
                                switch (str)
                                {
                                    case "фамилия":
                                        person.LastName = value;
                                        break;
                                    case "имя":
                                        person.FirstName = value;
                                        break;
                                    case "отчество":
                                        person.Surname = value;
                                        break;
                                    case "дата рождения":
                                        person.DateOfBirth = value;
                                        break;
                                    case "место рождения":
                                        person.PlaceOfBirth = value;
                                        break;
                                    case "инн":
                                        person.Inn = value;
                                        break;
                                    case "снилс":
                                        person.Snils = value;
                                        break;
                                    case "e-mail":
                                        person.Email = value;
                                        break;
                                    case "должность сотрудника (в соответствии с штатным расписанием)":
                                        person.Position = value;
                                        break;
                                    case "дата приказа":
                                        person.PrikazDate = value;
                                        break;
                                    case "номер приказа":
                                        person.PrikazNumber = value;
                                        break;
                                }

                                break;
                            case PersonDataGroup.organisation:
                                switch (str)
                                {
                                    case "огрн":
                                        organisation.Ogrn = value;
                                        break;
                                    case "инн":
                                        organisation.Inn = value;
                                        break;
                                    case "должность руководителя":
                                        break;
                                    // case "фио руководителя":
                                    //     organisation.MenagerName = value;
                                    //     break;
                                    // case "адрес":
                                    //     organisation.Address = value;
                                    //     break;
                                }

                                break;
                            case PersonDataGroup.passport:
                                switch (str)
                                {
                                    case "серия":
                                        passport.Series = value;
                                        break;
                                    case "номер":
                                        passport.Number = value;
                                        break;
                                    case "дата выдачи":
                                        passport.DateOfIssue = value;
                                        break;
                                    case "код подразделения":
                                        passport.CodeDivision = value;
                                        break;
                                    case "дата рождения":
                                        person.DateOfBirth = value;
                                        break;
                                    case "место рождения":
                                        person.PlaceOfBirth = value;
                                        break;
                                }

                                break;
                        }

                        break;
                }
            }

            var someFieldsMissed = false;
            var missedPersonFields = new StringBuilder("Пропущены поля в личных данных:");
            foreach (var field in person.GetType().GetFields())
            {
                var fieldValue = field.GetValue(person);
                //if string empty
                if (field.FieldType == typeof(String) && (fieldValue == null || fieldValue.ToString() == ""))
                {
                    //skip if ...
                    if (
                        (field.Name == "powerOfAttorneyNumber" && !isPowerOfAttorneyHasNumber) ||
                        (field.Name == "powerOfAttorneyDate")
                        && !isBassedOnDoverenost
                        //(field.Name == "powerOfAttorneyNumber" || field.Name == "powerOfAttorneyDate") &&
                        //!isBassedOnDoverenost
                    )
                        continue;
                    else
                    {
                        missedPersonFields.AppendLine().Append("  ").Append(field.Name);
                        someFieldsMissed = true;
                    }
                }
            }

            StringBuilder missedOrganisationFields = new StringBuilder("Пропущены поля в данных организации:");
            foreach (var field in organisation.GetType().GetFields())
            {
                var fieldValue = field.GetValue(organisation);
                if (field.FieldType == typeof(String) && (fieldValue == null || fieldValue.ToString() == ""))
                {
                    if ((field.Name == "address" && !isULrole) ||
                        ((field.Name == "menagerName" || field.Name == "managerPosition") && isFirstCertRelease))
                        continue;
                    else
                    {
                        missedOrganisationFields.AppendLine().Append("  ").Append(field.Name);
                        someFieldsMissed = true;
                    }
                }
            }

            StringBuilder missedPassportFields = new StringBuilder("Пропущены поля в паспортных данных:");
            foreach (var field in passport.GetType().GetFields())
            {
                var fieldValue = field.GetValue(passport);
                if (field.FieldType == typeof(String) && (fieldValue == null || fieldValue.ToString() == ""))
                {
                    missedPassportFields.AppendLine().Append("  ").Append(field.Name);
                    someFieldsMissed = true;
                }
            }

            if (someFieldsMissed)
            {
                MessageBox.Show(
                    ((missedPersonFields.ToString().Split('\n').Length > 1)
                        ? missedPersonFields.AppendLine()
                        : new StringBuilder())
                    .Append((missedOrganisationFields.ToString().Split('\n').Length > 1)
                        ? missedOrganisationFields.AppendLine()
                        : new StringBuilder())
                    .Append((missedPassportFields.ToString().Split('\n').Length > 1)
                        ? missedPassportFields
                        : new StringBuilder())
                    .ToString());
                IsNull = true;
            }
        }
    }
}