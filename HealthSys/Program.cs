﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthSystemSys
{
    // Generic Repository
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items;
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // Patient Class
    public class Patient
    {
        public int Id;
        public string Name;
        public int Age;
        public string Gender;

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }

    // Prescription Class
    public class Prescription
    {
        public int Id;
        public int PatientId;
        public string MedicationName;
        public DateTime DateIssued;

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }

    // HealthSystemApp Class
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new Repository<Patient>();
        private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Evelyn Domson-Appiah", 30, "Female"));
            _patientRepo.Add(new Patient(2, "Cornelius Yawson", 45, "Male"));
            _patientRepo.Add(new Patient(3, "Ama Montford", 55, "Female"));

            _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Amoxicillin", DateTime.Now.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Cough Syrup", DateTime.Now));
            _prescriptionRepo.Add(new Prescription(5, 2, "Vitamin C", DateTime.Now));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap.Clear();
            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("All Patients");
            foreach (var p in _patientRepo.GetAll())
            {
                Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Age: {p.Age}, Gender: {p.Gender}");
            }
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            Console.WriteLine($"Prescriptions for Patient ID {patientId}");
            if (_prescriptionMap.ContainsKey(patientId))
            {
                foreach (var pres in _prescriptionMap[patientId])
                {
                    Console.WriteLine($"- {pres.MedicationName} (Issued: {pres.DateIssued.ToShortDateString()})");
                }
            }
            else
            {
                Console.WriteLine("No prescriptions found for this patient.");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            HealthSystemApp app = new HealthSystemApp();

            app.SeedData();
            app.BuildPrescriptionMap();
            app.PrintAllPatients();

            while (true)
            {
                Console.Write("\nEnter Patient ID to view prescriptions (or 'q' to quit): ");
                string input = Console.ReadLine()?.Trim();

                if (input?.ToLower() == "q")
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }

                if (int.TryParse(input, out int pid))
                {
                    app.PrintPrescriptionsForPatient(pid);
                    break; // Exit after showing prescriptions
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid patient ID or 'q' to quit.");
                }
            }
        }
    }
}