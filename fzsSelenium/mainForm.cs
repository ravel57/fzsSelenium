using System;
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

namespace fzsSelenium {
	public partial class mainForm : Form {

		private string region;
		private string tofk;
		public string role;
		private string gender;
		private string releaseBasis = "доверенности";
		private bool firstCertRelease = true;
		private DataTable personDataTable;
		private static IWebDriver driver;

		public mainForm() {
			InitializeComponent();
		}

		private async void mainForm_Load(object sender, EventArgs e) {
			Task<bool> task = WaitForSettinngsDone();
			await task.ContinueWith(_ => { startFzs_button.Enabled = true; }, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private async Task<bool> WaitForSettinngsDone() {
			while (!(
				(cityMejReg_radioButton.Checked | cityMos_radioButton.Checked | cityMO_radioButton.Checked)
				& (roleFL_radioButton.Checked | roleUL_radioButton.Checked | roleUL2_radioButton.Checked)
				& ((openFileDialog.FileName != String.Empty & (genderM_radioButton.Checked | genderF_radioButton.Checked)) | !firstCertRelease)
			)) {
				await Task.Delay(100);
			}
			return true;
		}

		private async void selectFile_button_Click(object sender, EventArgs e) {
			openFileDialog.Filter = "Word Documents |*.docx;*.doc";
			if (openFileDialog.ShowDialog() == DialogResult.OK) {
				fileName_label.Text = openFileDialog.FileName;
				personDataTable = new DataTable();
				WordprocessingDocument wordDoc;
				bool usedTmpFile = false;
				try {
					wordDoc = WordprocessingDocument.Open(openFileDialog.FileName, false);
				}
				catch (IOException) {
					File.Copy(openFileDialog.FileName, openFileDialog.FileName += ".tmp");
					wordDoc = WordprocessingDocument.Open(openFileDialog.FileName, false);
					usedTmpFile = true;
				}
				Table table = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().First();
				IEnumerable<TableRow> rows = table.Elements<TableRow>();
				personDataTable.Columns.Add("Поле");
				personDataTable.Columns.Add("Значение");
				foreach (TableRow row in rows) {
					if (row.InnerText != "") {
						personDataTable.Rows.Add();
						TableCell[] cells = row.Descendants<TableCell>().ToArray();
						for (int i = 0; i < cells.Length; i++) {
							if (cells[i].InnerText != "")
								personDataTable.Rows[personDataTable.Rows.Count - 1][i] = cells[i].InnerText.Trim();
						}
					}
				}
				fileView_dataGridView.DataSource = personDataTable;
				wordDoc.Close();
				if (usedTmpFile) File.Delete(openFileDialog.FileName);
			}
		}

		private void city1_radioButton_CheckedChanged(object sender, EventArgs e) {
			region = "г. Москва";
			tofk = "9500";
		}

		private void city2_radioButton_CheckedChanged(object sender, EventArgs e) {
			region = "г. Москва";
			tofk = "7300";
		}

		private void city3_radioButton_CheckedChanged(object sender, EventArgs e) {
			region = "Московская область";
			tofk = "4800";
		}

		private void role1_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.role = "Сертификат должностного лица";
		}

		private void role2_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.role = "Сертификат юридического лица";
		}

		private void role3_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.role = "Сертификат юридического лица без ФИО";
		}

		private void genderM_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.gender = "мужской";
		}

		private void genderF_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.gender = "женский";
		}

		private void certTime1_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.firstCertRelease = true;
			startFzs_button.Enabled = false;
			this.mainForm_Load(sender, e);
			this.genderM_radioButton.Enabled = true;
			this.genderF_radioButton.Enabled = true;

		}

		private void certTime2_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.firstCertRelease = false;
			this.genderM_radioButton.Enabled = false;
			this.genderF_radioButton.Enabled = false;
		}

		private void powerOfAttorneyReleaseBasis_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.releaseBasis = "доверенности";
		}

		private void customReleaseBasis_radioButton_CheckedChanged(object sender, EventArgs e) {
			this.releaseBasis = customReleaseBasis_textBox.Text;
			if (customReleaseBasis_radioButton.Checked)
				this.customReleaseBasis_textBox.Focus();
		}

		private void customReleaseBasis_textBox_TextChanged(object sender, EventArgs e) {
			this.customReleaseBasis_radioButton.Checked = true;
			this.releaseBasis = customReleaseBasis_textBox.Text;
		}

		private void startFzs_button_Click(object sender, EventArgs e) {
			if (this.tofk == "9500" & !this.firstCertRelease)
				if (MessageBox.Show("Доверенность отвезена в казначейство?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
			PersonalData personData = new PersonalData(
				(DataTable)this.fileView_dataGridView.DataSource,
				role.Contains("юридического лица"),
				releaseBasis == "доверенности",
				powerOfAttorneyHasNumber_checkBox.Checked,
				firstCertRelease
			);
			if (personData.isNull)
				return;
			if (personData.person.position.Length > 140) {
				MessageBox.Show("Длительность должности превышает 140 симолов");
				return;
			}

			driver = new InternetExplorerDriver(".\\");
			driver.Manage().Window.Maximize();
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

			if (this.firstCertRelease)
				driver.Navigate().GoToUrl("https://fzs.roskazna.ru/public/requests/new");
			else {
				driver.Navigate().GoToUrl("https://lk-fzs.roskazna.ru/private/requests/new");
				while (Process.GetProcessesByName("iexplore").ToArray().Count(p => p.MainWindowTitle.Contains("WebDriver - Internet Explorer")) > 0) {
					Thread.Sleep(250);
				}
			}
			Thread.Sleep(3000);

			/// 1st page
			/// Город
			driver.FindElement(By.CssSelector(".select2-selection.select2-selection--single")).Click();
			driver.FindElements(By.ClassName("select2-results__option"))
					.First(city => city.Text.Equals(this.region)).Click();

			if (this.firstCertRelease) {
				driver.FindElements(By.TagName("label")).First(checkBox => checkBox.Text.Equals("Организация")).Click();
				Thread.Sleep(500);
				/// ОГРН
				driver.FindElement(By.Id("r-ogrn")).SendKeys(personData.organisation.ogrn);
				/// Ожидание ввода капчи
				do {
					driver.FindElement(By.Id("userCaptchaInput")).Clear();
					driver.FindElement(By.Id("userCaptchaInput")).Click();
					do {
						Thread.Sleep(400);
					} while (driver.FindElement(By.Id("userCaptchaInput")).GetAttribute("value").Length < 5);
					Thread.Sleep(100);
					driver.FindElements(By.CssSelector(".btn.btn-b"))
						.First(btn => btn.GetAttribute("onclick").Equals("submitCreateRequestFormWithCaptcha()")).Click();
					Thread.Sleep(1000);
				} while (driver.FindElements(By.Id("userCaptchaInput")).Count > 0);
				//} while (driver.FindElements(By.Id("request-block")).Count == 0);
			} else {
				// ТОФК
				driver.FindElement(By.Id("Tofk")).SendKeys(this.tofk);
				driver.FindElement(By.Id("tofk-picker-btn")).Click();
				do {
					Thread.Sleep(3000);
				} while (driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send")).Where(btn => btn.Text.Equals("ВЫБРАТЬ")).ToArray().Length == 0
				);
				driver.FindElements(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-send"))
					.First(btn => btn.Text.Equals("ВЫБРАТЬ")).Click();
				driver.FindElements(By.CssSelector(".btn.btn-b"))
					.First(btn => btn.GetAttribute("value").Equals("Далее")).Click();
				do {
					Thread.Sleep(1000);
				} while (driver.FindElements(By.Id("request-block")).Count == 0);
			}

			/// ТОФК page
			if (firstCertRelease) {
				if (driver.FindElements(By.Id("CertificateRequestDestination_TOFK")).Count > 0) {
					driver.FindElements(By.TagName("label"))
						.First(el => el.GetAttribute("for").Equals("CertificateRequestDestination_TOFK")).Click();
					//Thread.Sleep(100);
				}
				driver.FindElement(By.Id("Tofk")).SendKeys(this.tofk);
				driver.FindElement(By.Id("tofk-picker-btn")).Click();
				do {
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

			/// 2nd page
			Thread.Sleep(1500);
			//driver.FindElements(By.TagName("label")).First(checkBox => checkBox.Text.Equals(this.role)).Click();
			var tmp = driver.FindElements(By.TagName("label"));
			tmp.FirstOrDefault(el => el.Text == this.role).Click();
			Thread.Sleep(1500);
			driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.pen.ui-modal-open.add-documents-link")).Click();
			/// 2nd page 1st menu
			Thread.Sleep(5000);
			if (this.firstCertRelease) {
				driver.FindElement(By.Id("Series")).SendKeys(personData.passport.series);
				driver.FindElement(By.Id("Number")).SendKeys(personData.passport.number);
				driver.FindElement(By.Id("DateOfIssue")).SendKeys(personData.passport.dateOfIssue);
				driver.FindElement(By.Id("CodeDivision")).SendKeys(personData.passport.codeDivision);
				driver.FindElement(By.Id("DateOfBirth")).SendKeys(personData.person.dateOfBirth);
				driver.FindElement(By.Id("PlaceOfBirth")).SendKeys(personData.person.placeOfBirth);
				driver.FindElement(By.CssSelector(".ui-radiobutton.styled-radio"))
					.FindElements(By.TagName("label")).First(checkBox => checkBox.Text.Equals(this.gender)).Click();
			}
			driver.FindElement(By.CssSelector(".btn.btn-lg.btn-b.pointer")).Click();
			/// 2nd page 1st menu completed

			/// 2nd page 2nd menu
			Thread.Sleep(3000);
			driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.pen.ui-modal-open.add-request-link")).Click();
			Thread.Sleep(3000);
			if (this.firstCertRelease) {
				driver.FindElement(By.Id("LastName")).SendKeys(personData.person.lastName);
				driver.FindElement(By.Id("FirstName")).SendKeys(personData.person.firstName);
				driver.FindElement(By.Id("Surname")).SendKeys(personData.person.surname);
				try {
					driver.FindElement(By.Id("INN")).SendKeys(personData.person.inn.Replace(" ", ""));
				}
				catch { }
				driver.FindElement(By.Id("SNILS")).SendKeys(personData.person.snils);
				Clipboard.SetText(personData.person.email);
				driver.FindElement(By.Id("Mail")).SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Insert);
				driver.FindElement(By.Id("Position")).SendKeys(personData.person.position);
				if (this.role == "Сертификат юридического лица") {
					if (this.region == "г. Москва")
						driver.FindElement(By.Id("Locality")).SendKeys("г. Москва");
					try {
						driver.FindElement(By.Id("Address")).SendKeys(personData.organisation.address);
						driver.FindElement(By.Id("INNPersonal")).SendKeys(personData.person.inn);
					}
					catch { }
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
			/// 2nd page 2nd menu completed
			/// wait criptoPRO generate certificate	
			js.ExecuteScript("document.title = 'WaitCryptoPro-fzsSelenium'");
			driver.FindElement(By.Id("crtReq_container")).SendKeys(OpenQA.Selenium.Keys.End);
			do {
				Thread.Sleep(1000);
			} while (Process.GetProcessesByName("iexplore").ToArray().Count(p => p.MainWindowTitle.Contains("WaitCryptoPro-fzsSelenium")) > 0);
			//do { Thread.Sleep(1000); }
			//while (
			//	driver.FindElements(By.Id("generate-request-btn")).Count > 0
			//);


			/// save id and link
			Thread.Sleep(2500);
			string path = ".\\saves\\";
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			string requestCode = new StringBuilder()
				.Append(driver.FindElement(By.Id("request-number")).Text)
				.Append("\n")
				.Append(driver.FindElements(By.TagName("a")).First(checkBox => checkBox.Text.Equals("ссылке")).GetAttribute("href"))
				.Append($"\nОГРН: {personData.organisation.ogrn}\nИНН: {personData.organisation.inn}")
				.ToString();
			string now = DateTime.Now.ToString(new CultureInfo("ru-RU"));
			string name;
			if (this.firstCertRelease)
				name = personData.person.lastName + " " + personData.person.firstName;// + " " + personData.person.surname;
			else {
				name = driver.FindElement(By.Id("request-fio")).Text;
				name = name.Remove(name.IndexOf(' ', name.IndexOf(' ') + 1));
			}
			string fileName = new StringBuilder()
				.Append(path)
				.Append(name)
				.Append(" ")
				.Append(this.role.Substring(this.role.IndexOf(' ') + 1, 1).ToUpper().Replace('Д','Ф'))
				.Append("Л ")
				.Append(now.Replace(':', '-').Remove(now.Length - 4, 3))
				.Append(".txt")
				.ToString();
			File.WriteAllText(fileName, requestCode);


			/// 2nd page 3rd menu
			try {
				Thread.Sleep(3000);
				driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.add.ui-modal-open.add-application-link")).Click();
				driver.FindElement(By.CssSelector(".btn.btn-b.btn-sm.add.ui-modal-open.add-application-link")).Click();
			}
			catch { }
			Thread.Sleep(5000);
			driver.FindElement(By.Id("DocumentName")).SendKeys(releaseBasis);
			driver.FindElement(By.Id("PowerOfAttorneyDate")).SendKeys(personData.person.powerOfAttorneyDate);
			driver.FindElement(By.Id("PowerOfAttorneyNumber")).SendKeys(personData.person.powerOfAttorneyNumber);
			//if(driver.FindElement(By.Id("PositionOwner")).Text == "")
			//	driver.FindElement(By.Id("PositionOwner")).SendKeys(personData.person.position);
			if (!this.firstCertRelease 
				&& !driver.FindElement(By.Id("FullNameOwner")).GetAttribute("value").Contains(personData.person.lastName)
			) {
				driver.FindElement(By.Id("PositionManager")).SendKeys(personData.organisation.menagerName);
				string menagerName = personData.organisation.menagerName;
				menagerName = new StringBuilder()
					.Append(menagerName.Substring(menagerName.IndexOf(' ') + 1, 1))
					.Append(". ")
					.Append(menagerName.Substring(menagerName.IndexOf(' ', menagerName.IndexOf(' ') + 1) + 1, 1))
					.Append(". ")
					.Append(menagerName.Substring(0, menagerName.IndexOf(' ')))
					.ToString();
				driver.FindElement(By.Id("FullNameManager")).SendKeys(menagerName);
			}
			driver.FindElement(By.CssSelector(".btn.btn-b.ui-modal-valid.ui-modal-open")).Click();
			
			/// 2nd page 3rd print page
			if (firstCertRelease) {
				Thread.Sleep(2000);
				do {
					Thread.Sleep(500);
				} while (driver.FindElements(By.CssSelector(".btn.btn-g.ui-modal-close")).Count <= 13);
				//if (!this.firstCertRelease) {
				driver.FindElements(By.CssSelector(".btn.btn-b")).First(el => el.Text.Equals("ПЕЧАТЬ")).Click();
				js.ExecuteScript("document.title = 'WaitPrint-fzsSelenium'");
				Clipboard.SetText("Заявление " + name);
				//}
			}
			do {
				Thread.Sleep(5000);
			} while (Process.GetProcessesByName("iexplore").ToArray().Count(p => p.MainWindowTitle.Contains("WaitPrint-fzsSelenium")) > 0);
			///after print
			do {
				Thread.Sleep(500);
			} while (driver.FindElements(By.Id("DocumentPowerOfAttorneyCompany_DocumentDate")).Count == 0);
			try {
				if (!string.IsNullOrEmpty(personData.person.powerOfAttorneyDate)) {
					driver.FindElement(By.Id("DocumentPowerOfAttorneyCompany_DocumentDate"))
						.SendKeys(personData.person.powerOfAttorneyDate + OpenQA.Selenium.Keys.Enter);
				}
			}
			catch { }
		}


		private void Form_Closing(object sender, CancelEventArgs e) {
			foreach (var process in Process.GetProcessesByName("IEDriverServer")) {
				process.Kill();
			}
		}

		private void certTime3_radioButton_CheckedChanged(object sender, EventArgs e) {

		}
	}



	public class PersonalData {
		public bool isNull = false;
		public class Person {
			public string lastName;                 //Фамилия
			public string firstName;                //Имя
			public string surname;                  //Отчество
			public string dateOfBirth;              //Дата рождения
			public string placeOfBirth;             //Место рождения
			public string inn;                      //ИНН
			public string snils;                    //СНИЛС
			public string email;                    //E-mail
			public string position;                 //Должность сотрудника (в соответствии с штатным расписанием
			public string powerOfAttorneyDate;      //Дата доверенности
			public string powerOfAttorneyNumber;    //Номер доверенности
		}
		public class Organisation {
			public string ogrn;                     //огрн
			public string inn;                      //инн
			public string managerPosition;          //должность руководителя
			public string menagerName;              //фио руководителя
			public string address;                  //адрес
		}
		public class Passport {
			public string series;                   //Серия
			public string number;                   //Номер
			public string dateOfIssue;              //Дата выдачи
			public string codeDivision;             //Код подразделения
		}
		public Person person = new Person();
		public Organisation organisation = new Organisation();
		public Passport passport = new Passport();

		enum PersonDataGroup {
			person,
			organisation,
			passport
		}

		public PersonalData(DataTable dataTable, bool isULrole, bool isBassedOnDoverenost, bool isPowerOfAttorneyHasNumber, bool isFirstCertRelease) {
			PersonDataGroup header = new PersonDataGroup();
			foreach (DataRow row in dataTable.Rows) {
				string str = row.ItemArray[0].ToString().ToLower();
				string value = row.ItemArray[1].ToString();
				switch (str) {
					case "информация о сотруднике":
						header = PersonDataGroup.person; break;
					case "данные об организации":
						header = PersonDataGroup.organisation; break;
					case "паспортные данные (заполняется строго по паспорту)":
						header = PersonDataGroup.passport; break;
					default:
						switch (header) {
							case PersonDataGroup.person:
								switch (str) {
									case "фамилия":
										person.lastName = value; break;
									case "имя":
										person.firstName = value; break;
									case "отчество":
										person.surname = value; break;
									case "дата рождения":
										person.dateOfBirth = value; break;
									case "место рождения":
										person.placeOfBirth = value; break;
									case "инн":
										person.inn = value; break;
									case "снилс":
										person.snils = value; break;
									case "e-mail":
										person.email = value; break;
									case "должность сотрудника (в соответствии с штатным расписанием)":
										person.position = value; break;
									case "дата доверенности":
										person.powerOfAttorneyDate = value; break;
									case "номер доверенности":
										person.powerOfAttorneyNumber = value; break;
								}
								break;
							case PersonDataGroup.organisation:
								switch (str) {
									case "огрн":
										organisation.ogrn = value; break;
									case "инн":
										organisation.inn = value; break;
									case "должность руководителя":
										organisation.managerPosition = value; break;
									case "фио руководителя":
										organisation.menagerName = value; break;
									case "адрес":
										organisation.address = value; break;
								}
								break;
							case PersonDataGroup.passport:
								switch (str) {
									case "серия":
										passport.series = value; break;
									case "номер":
										passport.number = value; break;
									case "дата выдачи":
										passport.dateOfIssue = value; break;
									case "код подразделения":
										passport.codeDivision = value; break;
									case "дата рождения":
										person.dateOfBirth = value; break;
									case "место рождения":
										person.placeOfBirth = value; break;
								}
								break;
						}
						break;
				}
			}
			bool someFieldsMissed = false;
			StringBuilder missedPersonFields = new StringBuilder("Пропущены поля в личных данных:");
			var ads = person.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (var field in person.GetType().GetFields()) {
				var fieldValue = field.GetValue(person);
				//if string empty
				if (field.FieldType == typeof(String) && (fieldValue == null || fieldValue.ToString() == "")) {
					//skip if ...
					if (
							(field.Name == "powerOfAttorneyNumber" && !isPowerOfAttorneyHasNumber) || (field.Name == "powerOfAttorneyDate") 
							&& !isBassedOnDoverenost
							//(field.Name == "powerOfAttorneyNumber" || field.Name == "powerOfAttorneyDate") &&
							//!isBassedOnDoverenost
						)
						continue;
					else {
						missedPersonFields.AppendLine().Append("  ").Append(field.Name);
						someFieldsMissed = true;
					}
				}
			}
			StringBuilder missedOrganisationFields = new StringBuilder("Пропущены поля в данных организации:");
			foreach (var field in organisation.GetType().GetFields()) {
				var fieldValue = field.GetValue(organisation);
				if (field.FieldType == typeof(String) && (fieldValue == null || fieldValue.ToString() == "")) {
					if ((field.Name == "address" && !isULrole) ||
						((field.Name == "menagerName" || field.Name == "managerPosition") && isFirstCertRelease))
						continue;
					else {
						missedOrganisationFields.AppendLine().Append("  ").Append(field.Name);
						someFieldsMissed = true;
					}
				}
			}
			StringBuilder missedPassportFields = new StringBuilder("Пропущены поля в паспортных данных:");
			foreach (var field in passport.GetType().GetFields()) {
				var fieldValue = field.GetValue(passport);
				if (field.FieldType == typeof(String) && (fieldValue == null || fieldValue.ToString() == "")) {
					missedPassportFields.AppendLine().Append("  ").Append(field.Name);
					someFieldsMissed = true;
				}
			}
			if (someFieldsMissed) {
				MessageBox.Show(
					((missedPersonFields.ToString().Split('\n').Length > 1) ? missedPersonFields.AppendLine() : new StringBuilder())
					.Append((missedOrganisationFields.ToString().Split('\n').Length > 1) ? missedOrganisationFields.AppendLine() : new StringBuilder())
					.Append((missedPassportFields.ToString().Split('\n').Length > 1) ? missedPassportFields : new StringBuilder())
					.ToString());
				isNull = true;
			}
		}
	}
}
