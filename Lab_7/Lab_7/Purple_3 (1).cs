﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            //поля
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;
            private int _ind;
            //свойства
            public string Name => _name;
            public string Surname => _surname;
            private double TotalMark
            {
                get
                {
                    if (_marks == null) return 0;
                    double sum = 0;
                    foreach (double m in _marks)
                        sum += m;
                    return sum;
                }
            }

            private int TopPlace
            {
                get
                {
                    if (_places == null) return 0;
                    int minim = 0;
                    for (int i = 0; i < _places.Length; i++)
                        if (_places[i] < _places[minim])
                            minim = i;
                    return _places[minim];
                }
            }

            public double[] Marks

            {
                get
                {
                    if (_marks == null) return null;
                    double[] copy = new double[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;
                }
            }
            public int[] Places
            {
                get
                {
                    if (_places == null) return null;
                    int[] copy = new int[_places.Length];
                    Array.Copy(_places, copy, _places.Length);
                    return copy;
                }
            }
            public int Score
            {
                get
                {
                    if (_places == null) return 0;
                    int result = 0;
                    for (int i = 0; i < _places.Length; i++)
                    {
                        result += _places[i];

                    }
                    return result;
                }
            }

            //конструктор
            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _places = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
                _marks = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
                _ind = 0;
            }
            //доп методы
            private void SetPlace(int i, int j)
            {
                if (_places == null || i < 0 || i >= _places.Length) return;

                _places[i] = j;
            }
            public void Evaluate(double result)
            {
                if (_marks == null || _ind >= 7) return;
                if (result < 0 || result > 6) return;
                _marks[_ind] = result;
                _ind++;

            }
            //main методы
            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;
                for (int i = 0; i < 7; i++)
                {
                    Array.Sort(participants, (x, y) =>
                    {
                        double a = 0, b = 0;

                        if (x.Marks == null)
                            a = 0;
                        else
                            a = x.Marks[i];
                        if (y.Marks == null)
                            b = 0;
                        else
                            b = y.Marks[i];
                        double dif = a - b;
                        if (dif < 0)
                            return 1;
                        else if (dif > 0)
                            return -1;
                        else
                            return 0;
                    });

                    for (int j = 0; j < participants.Length; j++)
                        participants[j].SetPlace(i, j + 1);
                }
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;
                foreach (var x in array)
                {
                    if (x.Places == null) return;
                }
                Array.Sort(array, (x, y) =>
                {
                    if (x.Score == y.Score)
                    {
                        if (x.TopPlace == y.TopPlace)
                        {
                            double xy = x.TotalMark - y.TotalMark;
                            if (xy < 0) return 1;
                            else if (xy > 0) return -1;
                            else return 0;
                        }
                        return x.TopPlace - y.TopPlace;
                    }
                    return x.Score - y.Score;
                });
            }
            public void Print()
            {
                System.Console.WriteLine($"{_name} {_surname} {Score}");
            }
        }
        public abstract class Skating
        {
            private Participant[] _participants;
            protected double[] _moods;
            private int _ind = 0;
            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;
                    var copy = new Participant[_participants.Length];
                    Array.Copy(_participants, copy, _participants.Length);
                    return copy;
                }
            }
            public double[] Moods
            {
                get
                {
                    if (_moods == null) return null;
                    var copy = new double[_moods.Length];
                    Array.Copy(_moods, copy, _moods.Length);
                    return copy;
                }
            }
            public Skating(double[] moods, bool needModificate = true)
            {
                if (moods == null || moods.Length < 7) return;
                _moods = new double[7];

                Array.Copy(moods, _moods, 7);
                _participants = new Participant[0];
                if (needModificate == true) ModificateMood();
            }
            protected abstract void ModificateMood();

            public void Evaluate(double[] marks)
            {
                if (marks == null || _moods == null || marks.Length < _moods.Length ||
                    _ind == _participants.Length || _participants == null) return;

                for (int i = 0; i < _moods.Length; i++)
                {
                    if (marks[i] != null || _moods[i] != null)
                    {
                        _participants[_ind].Evaluate(marks[i] * _moods[i]);
                    }
                }
                _ind++;
            }
            public void Add(Participant participant)
            {
                if (_participants == null) return;
                var part1 = new Participant[_participants.Length + 1];
                Array.Copy(_participants, part1, _participants.Length);
                part1[part1.Length - 1] = participant;
                _participants = part1;
            }
            public void Add(Participant[] participants)
            {
                if (_participants == null || participants == null) return;
                int l = _participants.Length;
                var part2 = new Participant[l + participants.Length];
                Array.Copy(_participants, part2, l);
                Array.ConstrainedCopy(participants, 0, part2, l, participants.Length);
                _participants = part2;
            }
        }
        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { }
            protected override void ModificateMood()
            {
                if (_moods == null) return;
                for (int i = 0; i < _moods.Length; i++)
                {
                    _moods[i] += (i + 1) / 10.0;
                }
            }

        }
        public class IceSkating : Skating
        {
            public IceSkating(double[] moods, bool needModificate = true) : base(moods, needModificate) { }
            protected override void ModificateMood()
            {
                if (_moods == null) return;
                for (int i = 0; i < _moods.Length; i++)
                {
                    _moods[i] += (_moods[i] * (i + 1) / 100.0);
                }
            }
        }
    }
}
/*
namespace Lab_7
{
    public class Purple_3
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private int[] _places;
            private double[] _marks;
            private int _k;

            public string Name => _name;
            public string Surname => _surname;
            public int[] Places
            {
                get
                {
                    if (_places == null) return null;
                    int[] copy = new int[_places.Length];
                    Array.Copy(_places, copy, _places.Length);
                    return copy;
                }

            }
            public double[] Marks
            {
                get
                {
                    if (_marks == null) return null;
                    double[] copy = new double[_marks.Length];
                    Array.Copy(_marks, copy, _marks.Length);
                    return copy;

                }
            }

            public int Score
            {
                get
                {
                    if (_places == null) return 0;
                    return Places.Sum();
                }
            }
            private int TopPlace
            {
                get
                {
                    if (_places == null) return 0;
                    return Places.Min();
                }
            }
            private double TotalMark
            {
                get
                {
                    if (_marks == null) return 0;
                    return Marks.Sum();
                }
            }

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _k = 0;
                _places = new int[7];
                _marks = new double[7];
            }

            public void Evaluate(double result)
            {
                if (result < 0.0 || result > 6.0)
                {
                    Console.WriteLine("Оценка может быть только от 0 до 6");
                    return;
                }
                if (_marks == null || _k >= _marks.Length) return;
                _marks[_k] = result;
                _k++;
            }

            private void SetPlace(int judge, int place)
            {
                if (_places == null || judge < 0 || judge > _places.Length) return;
                _places[judge] = place;
            }

            public void Print()
            {
                Console.WriteLine($"Name: {Name}. Surname: {Surname}. TotalScore: {Score}. TopPlace: {TopPlace}. TotalMark: {TotalMark}");
            }
            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null) return;
                for (int j = 0; j < 7; j++)
                {
                    Array.Sort(participants, (x, y) =>
                    {
                        double a = 0, b = 0;

                        if (x.Marks == null)
                            a = 0;
                        else
                            a = x.Marks[j];
                        if (y.Marks == null)
                            b = 0;
                        else
                            b = y.Marks[j];
                        double raz = a - b;
                        if (raz < 0)
                            return 1;
                        else if (raz > 0)
                            return -1;
                        else
                            return 0;
                    });

                    for (int i = 0; i < participants.Length; i++)
                        participants[i].SetPlace(j, i + 1);
                }
            }
            public static void Sort(Participant[] array)
            {

                if (array == null) return;
                foreach (var x in array)
                {
                    if (x.Places == null) return;
                }
                Array.Sort(array, (x, y) =>
                {
                    if (x.Score == y.Score)
                    {
                        if (x.TopPlace == y.TopPlace)
                        {
                            double z = x.TotalMark - y.TotalMark;
                            if (z < 0) return 1;
                            else if (z > 0) return -1;
                            else return 0;
                        }
                        return x.TopPlace - y.TopPlace;
                    }
                    return x.Score - y.Score;
                });
            }

        }
        public abstract class Skating
        {
            protected Participant[] _participants;
            protected double[] _judges;

            public Participant[] Participants
            {
                get
                {
                    if (_participants == null) return null;

                    return _participants;
                }
            }

            public double[] Moods
            {
                get
                {
                    if (_judges == null) return null;

                    return _judges;
                }
            }

            public Skating(double[] judges, bool needModificate = true)
            {
                if (judges == null || judges.Length != 7) return;
                _judges = new double[7];
                if (needModificate == true) ModificateMood();


                for (int i = 0; i < Math.Min(judges.Length, 7); i++)
                {
                    _judges[i] = judges[i];
                }
                ModificateMood();
                _participants = new Participant[0];
            }

            protected abstract void ModificateMood();

            public void Evaluate(double[] marks)
            {
                if (marks == null || _participants == null || marks.Length < _judges.Length || _judges == null) return;
                for (int i = 0; i < _participants.Length; i++)
                {
                    if (_participants[i].Score == 0)
                    {
                        for (int j = 0; j < marks.Length; j++)
                        {
                            _participants[i].Evaluate(marks[j] * _judges[j]);
                        }
                        return;
                    }
                }
            }

            public void Add(Participant participant)
            {
                if (_participants == null) return;

                _participants = _participants.Append(participant).ToArray();

            }

            public void Add(Participant[] participant)
            {
                if (_participants == null || participant == null) return;

                _participants = _participants.Concat(participant).ToArray();

            }

        }


        public class FigureSkating : Skating
        {
            public FigureSkating(double[] _judges, bool needModificate = true) : base(_judges, needModificate) { }
            protected override void ModificateMood()
            {
                if (_judges == null) return;
                for (int i = 0; i < _judges.Length; i++)
                {
                    _judges[i] += (i + 1) / 10.0;
                }
            }
        }

        public class IceSkating : Skating
        {
            public IceSkating(double[] _judges, bool needModificate = true) : base(_judges, needModificate) { }
            protected override void ModificateMood()
            {
                if (_judges == null) return;
                for (int i = 0; i < _judges.Length; i++)
                {
                    _judges[i] += (_judges[i] * (1 + i) / 100.0);
                }
            }
        }

    }


}
*/
