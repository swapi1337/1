using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RPG
{

    public partial class Fight : Window
    {

        Character pers;
        Character enemy;
        string spellenemy;
        string parentDirectory = Environment.CurrentDirectory + "\\..\\..";
        List<string> historyDef = new List<string>(); // история обороны персонажа
        string[] names = new string[] { "Ужасный Каелах", "Свирепый Гуль", "Бескомпромисный Иврит"};

        // В главной функции создадим нашего персонажа и вызовем необходимые функции.

        public Fight(string Class, string Name)
        {
            InitializeComponent();
            
            pers = new Character(Class, Name);
            pers.WriteComboBox(cbSpell);
            CreateEnemy();
            Update();
        }

        // рндомный враг
        public void CreateEnemy()
        {
            Random f = new Random();
            int c = f.Next(names.Length);
            enemy = new Character(RandomClass(), names[c]);
            enemy.CreateLvl(pers);
        }


        // вывод инфы и блокировка умений(если нет маны)
        public void Update()
        {
            pers.GetInfo(tbY);
            enemy.GetInfo(tbE);

            if (pers.GiveMp())
            {
                cbSpell.IsEnabled = true;
                cbSpell.SelectedIndex = 0;

            }
            else
            {
                cbSpell.IsEnabled = false;
                cbSpell.SelectedIndex = 2;
            }

            SetImage(enemy.class_char, imgE);
            SetImage(pers.class_char, imgY);
        }

        //Теперь реализуем функцию RandomClass, которую вызываем из функции CreateEnemy.
        private string RandomClass()
        {
            Random rnd = new Random();
            int c = rnd.Next(1, 8);
            switch (c)
            {
                case 1: return "Warrior";
                case 2: return "Rogue";
                case 3: return "Mage";
                case 4: return "Sniper";
                case 5: return "Berserk";
                case 6: return "Assasin";
                case 7: return "Chaos";
                default: return "Warrior";
            }
        }

        // настройка аваторок
        public void SetImage(string a, Image img)
        {
            if (a == "Warrior")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\warrior.jpg"));
            else if (a == "Rogue")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\rogue.jpg"));
            else if (a == "Mage")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\mage.jpg"));
            else if (a == "Berserk")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\Berserk.jpg"));
            else if (a == "Sniper")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\Sniper.jpg"));
            else if (a == "Assasin")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\Assasin.jpg"));
            else if (a == "Chaos")
                img.Source = new BitmapImage(new Uri($"{parentDirectory}\\img\\Chaos.jpg"));
        }

        //логистика боя врага(ИИ)
        private string RandomMove()
        {
            historyDef.Add(cbDefence.Text);
            foreach (var DefNum in this.historyDef)
            {
                Random c = new Random();
                int d = c.Next(1, 5);
                if (DefNum == "Голова" )
                {
                    if (d == 1)
                        return "Грудь";
                    else if (d == 2)
                        return "Пах";
                    else if (d == 3)
                        return "Ноги";
                    else return "Голова";
                }
                else if (DefNum == "Грудь")
                {
                    if (d == 1)
                        return "Голова";
                    else if (d == 2)
                        return "Пах";
                    else if (d == 3)
                        return "Ноги";
                    else return "Грудь";
                }
                else if (DefNum == "Пах")
                {
                    if (d == 1)
                        return "Голова";
                    else if (d == 2)
                        return "Грудь";
                    else if (d == 3)
                        return "Ноги";
                    else return "Пах";
                }
                else if (DefNum == "Ноги")
                {
                    if (d == 1)
                        return "Голова";
                    else if (d == 2)
                        return "Пах";
                    else if (d == 3)
                        return "Грудь";
                    else return "Ноги";
                }
            }
             return "Голова"; 




        }

        private string RandomSpell(Character g)
        {

            Random rnd = new Random();
            int c = rnd.Next(1, 3);
            if ((enemy.GiveMp()) && (c == 1))
                return g.spells[0];

            else if ((enemy.GiveMp()) && (enemy.InfoHp()))
                return g.spells[1];

            else return "Нет";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            tbLog.Clear();
            spellenemy = RandomSpell(enemy);
            enemy.Attack(pers, enemy, cbAttack.Text, RandomMove(), cbSpell.Text, spellenemy, tbLog);
            pers.UseSpells(cbSpell.Text);
            pers.RestoreMP(cbSpell.Text);
            pers.Attack(enemy, pers, RandomMove(), cbDefence.Text, spellenemy, cbSpell.Text, tbLog);
            enemy.UseSpells(spellenemy);
            enemy.RestoreMP(spellenemy);
            Update();
            if (pers.IsDie())
            {
                MessageBoxResult f = MessageBox.Show("Ваш персонаж погиб, хотите восстановить персонажа и повторить?", "Вы погибли", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (f == MessageBoxResult.Yes)
                {
                    tbLog.Clear();
                    pers.Restore();
                    enemy.Restore();
                    pers.GetInfo(tbY);
                    enemy.GetInfo(tbE);
                }
            }
            if (enemy.IsDie())
            {
                MessageBoxResult f = MessageBox.Show("В этом бою вы победили, продолжить сражение?", "Победа", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (f == MessageBoxResult.Yes)
                {
                    tbLog.Clear();
                    pers.PlayerWin();
                    pers.Restore();
                    CreateEnemy();

                    Update();
                }
            }
        }
    }
}
