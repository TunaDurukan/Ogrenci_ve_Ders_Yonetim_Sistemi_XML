using System;
using System.Collections.Generic;
using Ogrenci_ve_Ders_Yonetim_Sistemi.Models;
using Ogrenci_ve_Ders_Yonetim_Sistemi.Helpers;

namespace Ogrenci_ve_Ders_Yonetim_Sistemi
{
    class Program
    {
        private const string StudentsFilePath = "students.xml";
        private const string InstructorsFilePath = "instructors.xml";
        private const string CoursesFilePath = "courses.xml";

        static List<Student> students = XMLHelper.LoadFromXml<List<Student>>(StudentsFilePath) ?? new List<Student>();
        static List<Instructor> instructors = XMLHelper.LoadFromXml<List<Instructor>>(InstructorsFilePath) ?? new List<Instructor>();
        static List<Courses> courses = XMLHelper.LoadFromXml<List<Courses>>(CoursesFilePath) ?? new List<Courses>();

        static void Main(string[] args)
        {
            InitializeData();

            while (true)
            {
                Console.WriteLine("\n=== Öğrenci ve Ders Yönetim Sistemi ===");
                Console.WriteLine("1- Öğrenci Yönetimi");
                Console.WriteLine("2- Öğretim Görevlisi Yönetimi");
                Console.WriteLine("3- Ders Yönetimi");
                Console.WriteLine("0- Çıkış");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine() ?? string.Empty;
                switch (choice)
                {
                    case "1":
                        StudentManagementMenu();
                        break;
                    case "2":
                        InstructorManagementMenu();
                        break;
                    case "3":
                        CourseManagementMenu();
                        break;
                    case "0":
                        Console.WriteLine("Çıkış yapılıyor...");
                        SaveAllData();
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                        break;
                }
            }
        }

        static void InitializeData()
        {
            if (courses.Count == 0)
            {
                var course1 = new Courses { CourseName = "Nesneye Dayalı Programlama", Credit = 3 };
                var course2 = new Courses { CourseName = "Görsel Programlama", Credit = 3 };
                courses.AddRange(new[] { course1, course2 });
                SaveAllData();
            }
        }

        static void SaveAllData()
        {
            XMLHelper.SaveToXml(StudentsFilePath, students);
            XMLHelper.SaveToXml(InstructorsFilePath, instructors);
            XMLHelper.SaveToXml(CoursesFilePath, courses);
        }

        static void StudentManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Öğrenci Yönetimi ---");
                Console.WriteLine("1- Öğrenci Ekle");
                Console.WriteLine("2- Öğrenci Sil");
                Console.WriteLine("3- Öğrenci Listesi");
                Console.WriteLine("4- Öğrenciye Ders Ata");
                Console.WriteLine("0- Ana Menüye Dön");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine() ?? string.Empty;
                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        RemoveStudent();
                        break;
                    case "3":
                        ListStudents();
                        break;
                    case "4":
                        AssignCourseToStudent();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                        break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Clear();
            Console.Write("Öğrenci Adı: ");
            string firstName = Console.ReadLine() ?? string.Empty;
            Console.Write("Öğrenci Soyadı: ");
            string lastName = Console.ReadLine() ?? string.Empty;
            Console.Write("E-posta: ");
            string email = Console.ReadLine() ?? string.Empty;
            Console.Write("Öğrenci Numarası: ");
            string studentNumber = Console.ReadLine() ?? string.Empty;

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                StudentNumber = studentNumber
            };

            students.Add(student);
            SaveAllData();
        }

        static void RemoveStudent()
        {
            Console.Clear();
            Console.WriteLine("\n--- Mevcut Öğrenciler ---");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].GetFullName()}");
            }

            Console.Write("Silmek istediğiniz öğrencinin numarasını seçin: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= students.Count)
            {
                students.RemoveAt(index - 1);
                SaveAllData();
            }
            else
            {
                Console.WriteLine("Geçersiz seçim.");
            }
        }

        static void ListStudents()
        {
            Console.Clear();
            Console.WriteLine("\n--- Öğrenci Listesi ---");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.GetFullName()}");
                if (student.EnrolledCourses.Count > 0)
                {
                    Console.WriteLine("Aldığı Dersler:");
                    foreach (var course in student.EnrolledCourses)
                    {
                        Console.WriteLine($"   - {course.CourseName} ({course.Credit})");
                    }
                }
                else
                {
                    Console.WriteLine("Aldığı Dersler: Henüz ders kaydı yok.");
                }
            }
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }

        static void AssignCourseToStudent()
        {
            Console.Clear();
            Console.WriteLine("\n--- Mevcut Öğrenciler ---");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].GetFullName()}");
            }

            Console.Write("Ders atamak istediğiniz öğrenciyi seçin: ");
            if (int.TryParse(Console.ReadLine(), out int studentIndex) && studentIndex > 0 && studentIndex <= students.Count)
            {
                var student = students[studentIndex - 1];
                Console.WriteLine("\n--- Mevcut Dersler ---");
                for (int i = 0; i < courses.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {courses[i].CourseName} - Kredi: {courses[i].Credit}");
                }

                Console.Write("Atamak istediğiniz dersi seçin: ");
                if (int.TryParse(Console.ReadLine(), out int courseIndex) && courseIndex > 0 && courseIndex <= courses.Count)
                {
                    var course = courses[courseIndex - 1];

                    if (!student.EnrolledCourses.Contains(course))
                    {
                        student.EnrolledCourses.Add(course);
                        course.EnrolledStudents.Add(student.StudentNumber);
                        SaveAllData();
                        Console.WriteLine($"Ders \"{course.CourseName}\" başarıyla \"{student.GetFullName()}\" adlı öğrenciye atandı.");
                    }
                    else
                    {
                        Console.WriteLine("Bu ders zaten öğrenciye atanmış.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz ders seçimi.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz öğrenci seçimi.");
            }
        }


        static void InstructorManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Öğretim Görevlisi Yönetimi ---");
                Console.WriteLine("1- Öğretim Görevlisi Ekle");
                Console.WriteLine("2- Öğretim Görevlisi Sil");
                Console.WriteLine("3- Öğretim Görevlisi Listesi");
                Console.WriteLine("4- Öğretim Görevlisine Ders Ata");
                Console.WriteLine("0- Ana Menüye Dön");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine() ?? string.Empty;
                switch (choice)
                {
                    case "1":
                        AddInstructor();
                        break;
                    case "2":
                        RemoveInstructor();
                        break;
                    case "3":
                        ListInstructors();
                        break;
                    case "4":
                        AssignCourseToInstructor();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                        break;
                }
            }
        }

        static void AddInstructor()
        {
            Console.Clear();
            Console.Write("Adı: ");
            string firstName = Console.ReadLine() ?? string.Empty;
            Console.Write("Soyadı: ");
            string lastName = Console.ReadLine() ?? string.Empty;
            Console.Write("E-Posta: ");
            string email = Console.ReadLine() ?? string.Empty;
            Console.Write("Departman: ");
            string department = Console.ReadLine() ?? string.Empty;

            var instructor = new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                Department = department,
                Email = email
            };

            instructors.Add(instructor);
            SaveAllData();
        }

        static void ListInstructors()
        {
            Console.Clear();
            Console.WriteLine("\n--- Öğretim Görevlisi Listesi ---");
            foreach (var instructor in instructors)
            {
                Console.WriteLine($"Ad: {instructor.GetFullName()} - Departman: {instructor.Department}");
                if (instructor.Courses.Count > 0)
                {
                    Console.WriteLine("   Verilen Dersler:");
                    foreach (var courseName in instructor.Courses)
                    {
                        Console.WriteLine($"   - {courseName}");
                    }
                }
                else
                {
                    Console.WriteLine("   Verilen Dersler: Henüz atanmadı.");
                }
            }
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }

        static void AssignCourseToInstructor()
        {
            Console.Clear();
            Console.WriteLine("\n--- Öğretim Görevlileri ---");
            for (int i = 0; i < instructors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {instructors[i].GetFullName()} - Departman: {instructors[i].Department}");
            }

            Console.Write("Ders atamak istediğiniz öğretim görevlisini seçin: ");
            if (int.TryParse(Console.ReadLine(), out int instructorIndex) && instructorIndex > 0 && instructorIndex <= instructors.Count)
            {
                var instructor = instructors[instructorIndex - 1];
                Console.WriteLine("\n--- Mevcut Dersler ---");
                for (int i = 0; i < courses.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {courses[i].CourseName} - Kredi: {courses[i].Credit}");
                }

                Console.Write("Atamak istediğiniz dersi seçin: ");
                if (int.TryParse(Console.ReadLine(), out int courseIndex) && courseIndex > 0 && courseIndex <= courses.Count)
                {
                    var course = courses[courseIndex - 1];
                    course.Instructor = instructor;
                    if (!instructor.Courses.Contains(course.CourseName))
                    {
                        instructor.Courses.Add(course.CourseName);
                    }
                    SaveAllData();
                    Console.WriteLine($"Ders \"{course.CourseName}\" başarıyla \"{instructor.GetFullName()}\" adlı öğretim görevlisine atandı.");
                }
                else
                {
                    Console.WriteLine("Geçersiz ders seçimi.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz öğretim görevlisi seçimi.");
            }
        }

        static void RemoveInstructor()
        {
            Console.Clear();
            Console.WriteLine("\n--- Mevcut Öğretim Görevlileri ---");
            for (int i = 0; i < instructors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {instructors[i].GetFullName()} - Departman: {instructors[i].Department}");
            }

            Console.Write("Silmek istediğiniz öğretim görevlisinin numarasını seçin: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= instructors.Count)
            {
                instructors.RemoveAt(index - 1);
                SaveAllData();
            }
            else
            {
                Console.WriteLine("Geçersiz seçim.");
            }
        }

        static void CourseManagementMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Ders Yönetimi ---");
                Console.WriteLine("1- Ders Ekle");
                Console.WriteLine("2- Ders Sil");
                Console.WriteLine("3- Ders Listesi");
                Console.WriteLine("0- Ana Menüye Dön");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine() ?? string.Empty;
                switch (choice)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        RemoveCourse();
                        break;
                    case "3":
                        ListCourses();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, tekrar deneyin.");
                        break;
                }
            }
        }

        static void AddCourse()
        {
            Console.Clear();
            Console.Write("Ders Adı: ");
            string courseName = Console.ReadLine() ?? string.Empty;
            Console.Write("Kredi: ");
            if (int.TryParse(Console.ReadLine(), out int credit))
            {
                var course = new Courses
                {
                    CourseName = courseName,
                    Credit = credit
                };

                courses.Add(course);
                SaveAllData();
            }
            else
            {
                Console.WriteLine("Geçersiz kredi değeri.");
            }
        }

        static void RemoveCourse()
        {
            Console.Clear();
            Console.WriteLine("\n--- Mevcut Dersler ---");
            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].CourseName} - Kredi: {courses[i].Credit}");
            }

            Console.Write("Silmek istediğiniz dersin numarasını seçin: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= courses.Count)
            {
                courses.RemoveAt(index - 1);
                SaveAllData();
            }
            else
            {
                Console.WriteLine("Geçersiz seçim.");
            }
        }

        static void ListCourses()
        {
            Console.Clear();
            Console.WriteLine("\n--- Mevcut Dersler ---");
            foreach (var course in courses)
            {
                Console.WriteLine($"Ders Adı: {course.CourseName} - Kredi: {course.Credit}");
                Console.WriteLine($"Öğretim Görevlisi: {(course.Instructor != null ? course.Instructor.GetFullName() : "Atanmamış")}");
                Console.WriteLine($"Kayıtlı Öğrenci Sayısı: {course.EnrolledStudents.Count}");
            }
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}
