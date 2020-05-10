using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    class Character
    {
        private int hp; // здоровье
        private int exp; // опыт
        private int mp; // мана
        private int def; // защита
        private int damage; //  урон
        private int lvl; // уровень
        public string name; // имя персонажа
        private int fullhp; // Максимальное количество здоровья
        public string class_char; // класс персонажа
        public string[] spells; // заклинания и способности
        private int fullmp; // максимум маны
        private bool attack; // true/false
        private int normDamage;
        private int normDef;
        private List<string> debuff = new List<string>(); // запись дебафа
        List<string> historyHp = new List<string>(); // запись текущего хп.
        private string[] allSkill = new string[] { "Сильный удар", "Поднять щит", "Вихрь", "Уворот", "Отравленный клинок", "Огненный шар", "Лечение", "Выстрел", "Выстрел дробью", "Жажда крови", "Ярость берсерка" };

        public Character(string Class, string Name)
        {
            this.lvl = 1;
            this.exp = 0;
            this.name = Name;
            this.class_char = Class;

            //прописываем характеристики классов

            if (Class == "Warrior")
            {
                this.hp = 500;
                this.fullhp = 500;
                this.fullmp = 10;
                this.mp = 10;
                this.damage = 14;
                this.def = 10;
                this.normDamage = damage;
                this.normDef = def;
                this.spells = new string[] { "Сильный удар", "Поднять щит" };
            }

            else if (Class == "Rogue")
            {
                this.hp = 300;
                this.fullhp = 300;
                this.fullmp = 30;
                this.mp = 30;
                this.damage = 18;
                this.def = 7;
                this.normDamage = damage;
                this.normDef = def;
                this.spells = new string[] { "Вихрь", "Уворот" };
            }

            else if (Class == "Mage")
            {
                this.hp = 200;
                this.fullhp = 200;
                this.fullmp = 100;
                this.mp = 100;
                this.damage = 11;
                this.def = 6;
                this.normDamage = damage;
                this.normDef = def;
                this.spells = new string[] { "Огненный шар", "Лечение" };

            }

            else if (Class =="Sniper")
            {
                this.hp = 250;
                this.fullhp = 250;
                this.fullmp = 20;
                this.mp = 20;
                this.damage = 30;
                this.def = 8;
                this.attack = true;
                this.normDamage = damage;
                this.normDef = def;
                this.spells = new string[] { "Выстрел", "Выстрел дробью" };

            }

            else if (Class == "Berserk")
            {
                this.hp = 450;
                this.fullhp = 450;
                this.fullmp = 20;
                this.mp = 20;
                this.damage = 28;
                this.def = 5;
                this.attack = true;
                this.normDamage = damage;
                this.normDef = def;
                this.spells = new string[] { "Жажда крови", "Ярость берсерка"};
            }
            else if (Class == "Assasin")
            {
                this.hp = 270;
                this.fullhp = 270;
                this.fullmp = 60;
                this.mp = 60;
                this.damage = 15;
                this.def = 3;
                this.attack = true;
                this.spells = new string[] { "Отравленный клинок", "Уворот" };
            }
            else if (Class == "Chaos")
            {
                Random g = new Random();
                this.hp = g.Next(200, 501);
                this.fullhp = this.hp;
                this.fullmp = g.Next(10, 151);
                this.mp = this.fullmp;
                this.damage = g.Next(14, 31);
                this.def = g.Next(6, 18);
                this.attack = true;
                this.normDamage = damage;
                string fst = this.allSkill[g.Next(0, allSkill.Length)];
                string sec;
                do
                {
                    sec = this.allSkill[g.Next(0, allSkill.Length)];
                } while (sec == fst);
                this.spells = new string[] { fst, sec};
            }
        }

        //  вывод информации о персонаже
        public void GetInfo(TextBox f)
        {
            f.Text = $"Имя -{this.name}\nКласс - {this.class_char}\nУровень -" +
                $"{this.lvl}\nОпыт - {this.exp}\nАтака - {this.damage}\nЗащита - {this.def}\nЗдоровье - " +
                $"{this.hp}/{this.fullhp}\nМана - {this.mp}/{this.fullmp}";
        }

        // функция для проверки кол-ва хп
        public bool InfoHp()
        {
           
           if (this.hp < (this.fullhp / 2 + 60))
                return false;
            return true;
            
        }

        //функция повышения уровня для бота

        public void CreateLvl(Character pers)
        {
            while (this.lvl < pers.lvl)
            {
                PlayerWin();
            }
               

           
        }

        // метод для заполнения ComboBox игрока способностями персонажа и поле для хода без использования способностей
        public void WriteComboBox(ComboBox a)
        {
            foreach (var g in this.spells)
            {
                a.Items.Add(g);
            }
            a.Items.Add("Нет");
        }
        //  расход маны при использовании способностей
        public void UseSpells(string a)
        {
            if (a != "Нет")
                this.mp -= 10;
        }

        //метод, который позволит проверить, хватает ли персонажу маны на способность. Т.к. все заклинания по 10 маны, будем сравнивать с этим значением.
        public bool GiveMp()
        {
            if (this.mp < 10)
                return false;
            return true;
        }
        
       
        //Далее метод проверки жив персонаж или мертв.
        public bool IsDie()
        {
            if (this.hp <= 0)
            {
                this.hp = 0;
                return true;
            }
            return false;
        }

        // 3 метода на восстановление персонажа, регенерацию маны и получение опыта или уровня при победе, который вызывает метод увеличения характеристик персонажа.
        public void Restore()
        {
            this.hp = this.fullhp;
            this.mp = this.fullmp;
            if(this.class_char == "Berserk" || this.class_char == "Chaos")
            {
                this.damage = this.normDamage;
            }
            this.debuff.Clear();
            
        }
        public void RestoreMP(string a)
        {
            if (a == "Нет")
                if (this.mp < this.fullmp)
                    if (this.class_char == "Mage")
                        this.mp += 5;
                    else if (this.class_char == "Sniper")
                        this.mp += 10;
                    else if (this.class_char == "Assasin")
                        this.mp += 6;
                    else
                        this.mp += 2;

                   
        }
        public void PlayerWin()
        {
            this.exp += 20;
            if(this.exp >= 100)
            {
                this.exp = 0;
                this.lvl++;
                LvlUp();
                Restore();
            }
        }

        // Сама функция повышения уровня
        private void LvlUp()
        {
            if(this.class_char == "Warrior")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 105);
                this.damage = Convert.ToInt32(this.damage + 6);
                this.def = Convert.ToInt32(this.def + 4);
                this.fullmp = Convert.ToInt32(this.fullmp + 8);

            }
            else if (this.class_char == "Rogue")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 90);
                this.damage = Convert.ToInt32(this.damage + 5);
                this.def = Convert.ToInt32(this.def + 1);
                this.fullmp = Convert.ToInt32(this.fullmp + 5);

            }

            else if (this.class_char == "Mage")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 55);
                this.damage = Convert.ToInt32(this.damage + 6);
                this.def = Convert.ToInt32(this.def + 2);
                this.fullmp = Convert.ToInt32(this.fullmp + 20);

            }

            else if (this.class_char == "Sniper")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 80);
                this.damage = Convert.ToInt32(this.damage + 10);
                this.def = Convert.ToInt32(this.def + 2);
                this.fullmp = Convert.ToInt32(this.fullmp + 10);
            }
            else if (this.class_char == "Berserk")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 125);
                this.damage = Convert.ToInt32(this.damage + 10);
                this.def = Convert.ToInt32(this.def + 3);
                this.fullmp = Convert.ToInt32(this.fullmp + 6);
            }
            else if (this.class_char == "Assasin")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 70);
                this.damage = Convert.ToInt32(this.damage + 24);
                this.def = Convert.ToInt32(this.def + 2);
                this.fullmp = Convert.ToInt32(this.fullmp + 5);
            }
            else if (this.class_char == "Chaos")
            {
                this.fullhp = Convert.ToInt32(this.fullhp + 100);
                this.damage = Convert.ToInt32(this.damage + 5);
                this.def = Convert.ToInt32(this.def + 3);
                this.fullmp = Convert.ToInt32(this.fullmp + 5);
            }
        }
        // Теперь реализуем алгоритм получения урона персонажем(или не получения при успешном блоке).
        private void GetDamage(Character a, Character b, string at, string def, TextBox log)
        {
            foreach (var c in this.debuff)
            {
                if (c == "Яд")
                {
                    this.hp -= a.damage;
                    log.AppendText($"{b.name} получает {a.damage} урона от яда\n");
                }
            }


            if (a.class_char != "Sniper")
            {
                if (at != def)
                {
                    int res = a.damage - b.def;
                    if (res < 1)
                        res = 1;
                    else
                        this.hp -= res;
                    log.AppendText($"{a.name} ударил в {at} {b.name} на {res} урона\n");

                }
                else
                    log.AppendText($"{b.name} заблокировал удар\n");
            }
            else
            {
                log.AppendText($"{a.name} перезаряжается\n");
            }
        }

        // Теперь реализуем механику способностей и ведения боя
        public void Attack(Character a, Character b, string at, string def, string spellAt, string spellDef, TextBox log)
        {
            if (spellAt == "Нет" && spellDef == "Нет")
            {
                GetDamage(a, b, at, def, log);
                return;
            }

            // Список защитных умений
            else
            {
                if (spellDef == "Поднять щит")
                {
                    log.AppendText($"{b.name} применил {spellDef}\n");

                    if (at != def)
                    {
                        log.AppendText($"{a.name} ударил в {at} {b.name} на 1 урона\n");

                    }
                    else
                        log.AppendText($"{b.name} заблокировал удар\n");

                }

                else if (spellDef == "Уворот")
                {
                    log.AppendText($"{b.name} применил {spellDef}\n");
                    log.AppendText($"{b.name} уклонился от удара\n");
                }

                else if (spellDef == "Лечение")
                {
                    log.AppendText($"{b.name} применил {spellDef}\n");

                    this.hp += this.damage * 3;

                    if (this.hp > this.fullhp)
                        this.hp = this.fullhp;
                    if (at != def)
                    {
                        int res = a.damage - b.def;
                        if (res < 1)
                            res = 1;
                        else
                            this.hp -= res;
                        log.AppendText($"{a.name} ударил в {at} {b.name} на {res} урона\n");
                    }
                    else
                        log.AppendText($"{b.name} заблокировал удар\n");
                }

                else if (spellAt == "Выстрел") // СНАЙПЕР СПИСОК ЗАЩИТНЫХ!!!!!!!!!!
                {
                    log.AppendText($"{a.name} применил {spellAt}\n");
                    if (at != def)
                    {
                        int res = (a.damage - b.def)*4;
                        if (res < 1)
                            res = 1;
                        else
                            this.hp -= res;
                        log.AppendText($"{a.name} прострелил {at} {b.name} на {res} урона\n");
                    }
                    else
                    {
                        int res = (a.damage - b.def)*2;
                        if (res < 1)
                            res = 1;
                        else
                            this.hp -= res;
                        log.AppendText($"{a.name} прострелил {at} {b.name} на {res} урона\n");
                    }
                } 


                else if (spellDef == "Ярость берсерка")
                {
                    log.AppendText($"{b.name} применил {spellDef}\n");

                    int c = this.hp * 30 / 100;
                    this.hp -= c;
                    int f = this.damage * 50 / 100;
                    this.damage += f;
                    log.AppendText($"{b.name} режет себя, получая  {c} урона и увеличивает свою атаку на  {f}\n");
                }

                // Список атакующих умений

                else
                {
                    if (spellAt == "Сильный удар")
                    {
                        log.AppendText($"{a.name} применил {spellAt}\n");

                        if (at != def)
                        {
                            int res = a.damage * 3 - b.def;
                            if (res < 1)
                                res = 1;
                            else
                                this.hp -= res;
                            log.AppendText($"{a.name} ударил в {at} {b.name} на {res} урона\n");

                        }
                        else
                        {
                            int res = a.damage - b.def;
                            if (res < 1)
                                res = 1;
                            else
                                this.hp -= res;
                            log.AppendText($"{b.name} получил  {res} урона через блок\n");
                        }
                    }

                    if (spellAt == "Вихрь")
                    {
                        log.AppendText($"{a.name} применил {spellAt}\n");
                        if (at != def)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int res = (a.damage - b.def) * 2;

                                if (res < 1)
                                    res = 1;
                                else
                                    this.hp -= res;
                                log.AppendText($"{a.name} ударил в {at} {b.name} на {res} урона\n");
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                this.hp -= 1;
                                log.AppendText($"{a.name} ударил в {at} {b.name} через блок на 1 урона\n");
                            }
                        }

                    }

                    if (spellAt == "Выстрел дробью" )  //СНАЙПЕР СПИСОК АТАКИ
                    {
                        log.AppendText($"{a.name} применил {spellAt}\n");
                        if (at !=def)
                        {
                            Random f = new Random();
                            int c = f.Next(1, 10);
                            int res = (a.damage - b.def) * c;
                            if (res < 1)
                                res = 1;
                            else
                                this.hp -= res;
                            log.AppendText($"{a.name} прострелил {at} {b.name} на {res} урона\n");
                        }
                        else
                        {
                            int res = (a.damage - b.def) * 2;
                            if (res < 1)
                                res = 1;
                            else
                                this.hp -= res;
                            log.AppendText($"{a.name} прострелил {at} {b.name} на {res} урона\n");
                        }
                    }

                    if (spellAt == "Жажда крови") 
                    {
                        log.AppendText($"{b.name} применил {spellAt}\n");

                        b.hp = b.hp + b.damage;
                        log.AppendText($"{b.name} восполняет здоровье, вкусив кровь врага на {b.damage}\n");
                        GetDamage(a, b, at, def, log);
                    }

                    if (spellAt == "Отравленный клинок")
                    {
                        log.AppendText($"{a.name} применил {spellAt}\n");

                        if (at != def)
                        {
                            this.debuff.Add("Яд");
                            this.hp -= a.damage;
                            log.AppendText($"{a.name} ударил отравленным клинком беднягу {b.name} в {at}. {b.name} получает {a.damage} урона от яда\n");
                        }
                        else
                        {
                            log.AppendText($"{a.name} сломал нож об броню {b.name}\n");
                        }
                    }

                    if (spellAt == "Огненный шар")
                    {
                        log.AppendText($"{a.name} применил {spellAt}\n");
                        this.hp -= this.damage;
                        log.AppendText($"{a.name} спалил {at} {b.name} и нанес {this.damage} урона\n");
                    }
                    GetDamage(a, b, at, def, log);

                }

                
            }
        }
    }
}
