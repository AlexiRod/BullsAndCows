using System;
using System.Diagnostics;
using System.Linq;

namespace BullsAndCows
{
    class Program
    {
        private static void Rules()
        {
            Console.WriteLine("Приветствую в игре \"Быки и коровы\"! Правила игры таковы:" +
                    "\n - Компьютер загадывает n-значное число, все цифры которого различны. " +
                    "\n - Затем вы вводите свое n-значное число с различными цифрами, пытаясь определить загаданное число. " +
                    "\n - Если цифра в вашем числе стоит на той же позиции, что и в загаданном числе, она считается быком. " +
                    "\n - Если цифра из вашего числа есть в загаданном числе, но не стоит на той же позиции, она считается коровой. " +
                    "\n - После каждого введенного вами числа, вы будете получать число быков и коров для него. " +
                    "\n - Ваша цель - определить загаданное число, то есть получить n быков." +
                    "\n - Учитывайте также, что в числе не может быть более 10 или менее 1 цифры, первая его цифра не может быть нулем.");
        }

        static void Main(string[] args)
        {
            Rules();    
            
            do
            {
                Play();
                Console.Write("Для повторной игры введите \"1\". Для выхода введите любую другую строку: ");
            } 
            while (Console.ReadLine() == "1");

            Console.WriteLine("Программа успешно завершена. Хорошего дня!");
            Console.ReadLine();
        }

        /// <summary>
        /// Основной метод игры.
        /// </summary>
        private static void Play()
        {
            int n;

            Console.Write("Введите длину числа n, которое загадает компьютер. Его цифры будут различны, поэтому n должно быть натуральным числом от 1 до 10. Первая его цифра не равна нулю: ");
            while (!GetCorrectLength(Console.ReadLine(), out n))
            {
                Console.Write("Длина числа должна быть натуральным числом от 1 до 10. Введите другое n: ");
            }

            Console.WriteLine($"Компьютер успешно загадал {n}-значное число. Теперь попробуйте угадать его, основываясь на подсчете быков и коров для разных чисел.");

            string computerNum = GenerateRandomNumber(n); Debug.Write(computerNum);// Загаданное число. Для отладки программы оно выводится в окно Debug.
            string userNum; // Число, вводимое пользователем.
            int bulls = 0, cows = 0; // Быки и коровы.

            while (bulls != n)
            {
                bulls = 0; cows = 0; // Обнуление значений для нового рассчета.

                // Ввод числа пользователем.
                Console.Write($"Введите {n}-значное натуральное число с различными цифрами, не начинающееся с нуля для подсчета быков и коров: ");
                userNum = Console.ReadLine();

                while (!GetCorrectNumber(userNum, n)) // Проверка на корректность.
                {
                    Console.Write($"Число должно быть натуральным, состоять из {n} различных цифр и не должно начинаться с нуля. Введите другое число: ");
                    userNum = Console.ReadLine();
                }

                CountBullsAndCows(computerNum, userNum, ref bulls, ref cows);
                Console.WriteLine($"Быков: {bulls}. Коров: {cows}.");
            }
            Console.WriteLine($"Поздравляю! Вы отгадали загаданное число {computerNum}!");
        }

        /// <summary>
        /// Метод для подсчета быков и коров для введенного и поданного числа.
        /// </summary>
        /// <param name="computerNum">Загаданное число</param>
        /// <param name="userNum">Введенное число</param>
        /// <param name="bulls">Быки</param>
        /// <param name="cows">Коровы</param>
        private static void CountBullsAndCows(string computerNum, string userNum, ref int bulls, ref int cows)
        {
            char[] nums = userNum.ToCharArray(); // Разбиение числа на его цифры.

            for (int i = 0; i < userNum.Length; i++) // Поиск быков.
            {
                // Если цифра встретилась в обоих числах на одном и том же месте, то убирем ее из цифр числа 
                // (заменим на минус), чтобы не посчитать его за корову в будущем.
                if (userNum[i] == computerNum[i])
                {
                    bulls++;
                    nums[i] = '-';
                }
            }

            foreach (var num in nums) // Подсчет коров. Цифры-быки уже подсчитаны и заменены на минусы, поэтому коровами они считаться не будут.
            {
                if (computerNum.Contains(num)) // Если цифра введенного числа содержится в загаданном числе, увеличиваем число коров.
                {
                    cows++;
                }
            }
        }

        /// <summary>
        /// Метод для проверки корректности введенного пользователем числа.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static bool GetCorrectNumber(string num, int n)
        {
            // Строка должна являться натуральным числом, первая цифра которого не ноль и все цифры которого различны.
            if (num.Length != 0 && num[0] == '0' || !long.TryParse(num, out long outVal) || outVal < 0 || num.Length != n)
                return false;

            char[] nums = num.ToCharArray();  // Разбиение числа на его цифры.
            if (nums.Distinct().Count() == n) // Метод для определения количества различных элементов в массиве (различные цифры в числе).
                return true;

            return false;
        }

        /// <summary>
        /// Метод для проверки корректности введенной пользователем длины числа.
        /// </summary>
        /// <param name="s">Введенная строка</param>
        /// <param name="a">Длина строки</param>
        /// <returns></returns>
        private static bool GetCorrectLength(string s, out int n)
        {
            if (!int.TryParse(s, out n) || n < 1 || n > 10)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Метод для создания n-значного числа, все цифры которого различны.
        /// </summary>
        /// <param name="n"> Длина числа </param>
        /// <returns></returns>
        private static string GenerateRandomNumber(int n)
        {
            Random random = new Random();
            string num = ""; // Итоговое число. Тип string для использования метода Contains.

            for (int i = 0; i < n; i++) // Создание цифр.
            {
                int character;
                do // Генерируем новую цифру, пока не получим ту, что еще не была в записи числа.
                {
                    character = random.Next(0, 10); // В случае, если цифра первая и сгенерирован 0, операция повторяется.
                }                                                                         // Метод проверки того, содержит ли одна строка другую,
                while (num.Contains(character.ToString()) || (i == 0 && character == 0)); // В данном случае - содержит ли итоговое число сгенерированную цифру.
                num += character.ToString();
            }

            return num;
        }
    }
}
