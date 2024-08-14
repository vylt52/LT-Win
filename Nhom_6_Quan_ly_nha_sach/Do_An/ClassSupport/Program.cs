using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassSupport
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "Hello"; // Chuỗi mà bạn muốn kiểm tra

            // Biểu thức chính quy kiểm tra chuỗi chỉ chứa chữ và số
            string pattern = "^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);

            // Kiểm tra xem chuỗi có khớp với biểu thức chính quy không
            if (regex.IsMatch(input))
            {
                Console.WriteLine("Chuỗi chỉ chứa chữ và số.");
            }
            else
            {
                Console.WriteLine("Chuỗi chứa ký tự không phải chữ và số.");
            }
            Console.ReadKey();

        }
    }
}
