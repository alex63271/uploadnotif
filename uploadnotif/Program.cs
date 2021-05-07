using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace uploadnotif
{
    class Program
    {


        static string xmlUZ()  // метод создания уведомления
        {

            string s2 = Convert.ToString(Guid.NewGuid());   //генерация гуида уведомления
            DateTime BirthDatePledgor = new DateTime(1980, 12, 12); //задание даты рождения залогодателя
            DateTime DateContract = new DateTime(2005, 09, 12);
            DateTime BirthDateApplicant = new DateTime(1970, 04, 14);
            XNamespace xNs = "http://fciit.ru/eisn/ruzdi/types/2.3"; //создание нэймспейса для документа


            XDocument xdoc = new XDocument(
        new XElement(xNs + "PledgeNotificationToNotary", // описание тега PledgeNotificationToNotary
            new XAttribute("version", "2.3"),
            new XAttribute("xmlns", "http://fciit.ru/eisn/ruzdi/types/2.3"),

        new XElement(xNs + "NotificationId", s2),             // описание тега NotificationId

        new XElement(xNs + "NotificationData",                        // описание тега NotificationData
            new XAttribute("version", "2.3"),


        new XElement(xNs + "FormUZ1",                                 // описание тега FormUZ1

        new XElement(xNs + "PersonalProperties",                        //описание имущества
        new XElement(xNs + "PersonalProperty",
        new XElement(xNs + "VehicleProperty",
        new XElement(xNs + "VIN", "XTA21091111111111"),
        new XElement(xNs + "Description", "какой-то авто")))),


        new XElement(xNs + "Pledgors",                                  //описание залогодателя
        new XElement(xNs + "Pledgor",
        new XElement(xNs + "PrivatePerson",
        new XElement(xNs + "Name",
        new XElement(xNs + "Last", "Иванов"),
        new XElement(xNs + "First", "Иван"),
        new XElement(xNs + "Middle", "Ианович")),
        new XElement(xNs + "BirthDate", BirthDatePledgor.ToString("yyyy-MM-dd")),
        new XElement(xNs + "PersonDocument",
        new XElement(xNs + "Code", "21"),
        new XElement(xNs + "Name", "Паспорт гражданина РФ"),
        new XElement(xNs + "SeriesNumber", "1234567890")),
        new XElement(xNs + "PersonAddress",
        new XElement(xNs + "AddressRF",
        new XAttribute("registration", "true"),
        new XElement(xNs + "RegionCode", "21"),
        new XElement(xNs + "Region", "Чувашия")))))),



        new XElement(xNs + "Pledgee",                                    //описание залогодержателя
        new XElement(xNs + "Pledgee",
        new XElement(xNs + "Organization",
        new XElement(xNs + "RussianOrganization",
        new XElement(xNs + "NameFull", "ООО «Фонд Центр инноваций и информационных технологий»"),
        new XElement(xNs + "OGRN", "1107799007425"),
        new XElement(xNs + "INN", "7708237747"),
        new XElement(xNs + "Address",
        new XElement(xNs + "RegionCode", "21"),
        new XElement(xNs + "Region", "Чувашия")))))),



        new XElement(xNs + "PledgeContract",                 //описание договора залога
        new XElement(xNs + "Name", "Наименование договора залога"),
        new XElement(xNs + "Date", DateContract.ToString("yyyy-MM-dd")),
        new XElement(xNs + "TermOfContract", "Срок исполнения")),





        new XElement(xNs + "NotificationApplicant",              //описание заявителя
        new XElement(xNs + "Role", "2"),
        new XElement(xNs + "Organization",
        new XElement(xNs + "NameFull", "ООО «Фонд Центр инноваций и информационных технологий»"),
        new XElement(xNs + "URN", "1107799007425"),
        new XElement(xNs + "UINN", "7708237747"),
        new XElement(xNs + "Email", "123@qwert.ru")),
        new XElement(xNs + "Attorney",
        new XElement(xNs + "Name",
        new XElement(xNs + "Last", "Новосельцев"),
        new XElement(xNs + "First", "Алексей"),
        new XElement(xNs + "Middle", "Сергеевич")),
        new XElement(xNs + "BirthDate", BirthDateApplicant.ToString("yyyy-MM-dd")),

        new XElement(xNs + "Authority", "веские"),
        new XElement(xNs + "PersonDocument",
        new XElement(xNs + "Code", "21"),
        new XElement(xNs + "Name", "Паспорт гражданина РФ"),
        new XElement(xNs + "SeriesNumber", "1234567890")),
        new XElement(xNs + "PersonAddress",
        new XElement(xNs + "AddressRF",
        new XAttribute("registration", "true"),
        new XElement(xNs + "RegionCode", "21"),
        new XElement(xNs + "Region", "Чувашия")))))

                                                        ))));

            ///////////////////////////////////



            string s1 = "C:/Users/Admin/Desktop/createxml/"; // основная папка

            string s3 = s1 + s2;    //основная папка + гуид уведомления
            Directory.CreateDirectory(s3);  // создаем папку куда потом сохраним уведомление
            string s4 = String.Concat(s3, "/", s2, ".xml"); // даём имя и путь xml-файлу            
            xdoc.Save(s4);   //сохраняем xml - документ
            Console.WriteLine(s4);
            GC.Collect(2);
            return s2;


        }
        static bool signatur(string NotificationId)   // метод подписания уведомлений
        {

            string csp = "\"C:/Program Files/Crypto Pro/CSP/csptest.exe\"";
            string put = "C:/Users/Admin/Desktop/createxml/" + NotificationId + "/" + NotificationId;

            /* ProcessStartInfo psiOpt = new ProcessStartInfo("cmd.exe", "/C " + csp + " -sfsign -sign -detached -add -in " + put + ".xml -out " + put + ".xml.sig -my 1107799007425");

             // скрываем окно запущенного процесса         
             psiOpt.WindowStyle = ProcessWindowStyle.Hidden;
             psiOpt.RedirectStandardOutput = true;
             psiOpt.UseShellExecute = false;
             psiOpt.CreateNoWindow = true;
             // запускаем процесс
             Process procCommand = Process.Start(psiOpt);

             // закрываем процесс

             procCommand.WaitForExit();*/


            Process psiOpt = Process.Start(new ProcessStartInfo
            {

                FileName = "cmd",
                Arguments = "/C " + csp + " -sfsign -sign -detached -add -in " + put + ".xml -out " + put + ".xml.sig -my 37be916fb4ac808252940031b7a67dda2da223e3",
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });
            //psiOpt.StandardOutput.ReadToEnd();
           
            psiOpt.WaitForExit();
            GC.Collect(2);
            if (psiOpt.ExitCode != 0)
            {
                Console.WriteLine($" эксит код - {psiOpt.ExitCode}");
                Console.WriteLine("------------------------");
                ;
                return false;
            }
            else
            {
              
                Console.WriteLine($"{put}.xml.sig");
                return true;
            }
        }
        static string compr(string NotificationId)  // метод создания zip архива
        {

            string sourceFile = "C:/Users/Admin/Desktop/createxml/" + NotificationId + "/"; // исходная папка
            string compressedFile = "C:/Users/Admin/Desktop/createzip/" + NotificationId + ".zip"; // сжатый файл       
            ZipFile.CreateFromDirectory(sourceFile, compressedFile, CompressionLevel.Optimal, false);   // создание архива
            Console.WriteLine("архивировние завершено");
            GC.Collect(2);
            return compressedFile;  //возвращаем путь к архиву
        }

        static upl.uploadNotificationPackageRequest Pack(string putzip, string NotificationId) //метод формирования пакета
        {
            upl.uploadNotificationPackageRequest package = new upl.uploadNotificationPackageRequest();
            package.pledgeNotificationPackage = new upl.pledgeNotificationPackageType();
            upl.pledgeNotificationListElementType pledgeNotificationListElement = new upl.pledgeNotificationListElementType();
            upl.senderTypeType sender = 0;
            string guidp = Convert.ToString(Guid.NewGuid()); //генерируем гуид пкета
            package.pledgeNotificationPackage.packageId = guidp; //присваиваем пакету гуид
            package.pledgeNotificationPackage.senderType = sender; // присваиваем занчение senderType(0)
            package.pledgeNotificationPackage.uip = "000000000000000000000TEST"; //прописываем УИП
            Console.WriteLine($"гуид пакета - {guidp}"); //выводим на экран гуид пакета
            Console.WriteLine($"гуид уведомления - {NotificationId}");        //выводим на экран гуид уведомления    
            pledgeNotificationListElement.notificationId = NotificationId; // прописываем гуид уведомления

            FileStream fstream = File.OpenRead(putzip); //берем архив

            // преобразуем строку в байты
            byte[] array = new byte[fstream.Length]; //создаем массив байтов длинною в поток архива
            // считываем данные
            fstream.Read(array, 0, array.Length); //записываем в массив байты из архива
            fstream.Close();


            pledgeNotificationListElement.documentAndSignature = array; //передаем массив байтов в тег documentAndSignature


            upl.pledgeNotificationListElementType[] massiv = new upl.pledgeNotificationListElementType[20]; //создается массив из уведомлений до 20 шт.
            massiv[0] = pledgeNotificationListElement; //в первый элемент массива записываем сформированное уведомление
            package.pledgeNotificationPackage.pledgeNotificationList = massiv; //передаем массив уведомлений(1-20) в пакет
            GC.Collect(2);
            return package; //возвращаем значение для Task-отправки пакета

        }

        static async Task UploadAsync(upl.uploadNotificationPackageRequest package) //метод отправки пакета
        {
            upl.ruzdiUploadNotificationPackageService_v1_1PortTypeClient.EndpointConfiguration s = 0; //выбираем Endpoint первый попавшийся
            upl.ruzdiUploadNotificationPackageService_v1_1PortTypeClient request = new upl.ruzdiUploadNotificationPackageService_v1_1PortTypeClient(s); // создаем запрос на выбранный Endpoint

            upl.uploadNotificationPackageResponse response = await request.uploadNotificationPackageAsync(package); // отправляем пакет асинхронно
            

            Console.WriteLine($"код ошибки - {response.packageStateCode.code}");
            Console.WriteLine(response.packageStateCode.message);
            Console.WriteLine($"рег № пакета -  {response.registrationId}");
            GC.Collect(2);
            
        }

        static void not_sign(int n)   // метод сборка
        {
            n = 1;

            string NotificationId = xmlUZ();    //вызывем генерацию xml

            bool rez = signatur(NotificationId);   //подписываем xml
            if (rez == true)
            {
                string putzip = compr(NotificationId);  //архивируем xml и sig
                upl.uploadNotificationPackageRequest package = Pack(putzip, NotificationId);    // формируем пакет

                Task.Run(() => UploadAsync(package));   //отправляем пакет и получаем результат
                GC.Collect(2);
            }
           /* else            
            {
                Console.WriteLine($"ошибка формирования подписи");
            }*/
        }

        static void Main()
        {

            Console.WriteLine($"Сколько уведомлений сформировать?");

            int n = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"=========================================");
            Parallel.For(0, n, not_sign); // параллельный вызов метода сборки  
            GC.Collect(2);
            Console.ReadLine();
        }
    }
}
