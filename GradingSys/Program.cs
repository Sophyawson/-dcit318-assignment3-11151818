using System;
using System.Collections.Generic;
using System.IO;

// Custom Exceptions
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message) : base(message) { }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message) : base(message) { }
}

// Student Class
public class Student
{
    public int Id;
    public string FullName;
    public int Score;

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100) return "A";
        if (Score >= 70) return "B";
        if (Score >= 60) return "C";
        if (Score >= 50) return "D";
        return "F";
    }
}

// Processor Class
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        var students = new List<Student>();

        using (StreamReader reader = new StreamReader(inputFilePath))
        {
            string line;
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                string[] parts = line.Split(',');

                // Check number of fields
                if (parts.Length < 3)
                    throw new MissingFieldException($"Line {lineNumber}: Missing fields.");

                try
                {
                    int id = int.Parse(parts[0].Trim());
                    string name = parts[1].Trim();
                    int score = int.Parse(parts[2].Trim());

                    students.Add(new Student(id, name, score));
                }
                catch (FormatException)
                {
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format.");
                }
            }
        }

        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
            }
        }
    }
}

// Main Program
class Program
{
    static void Main()
    {
        string inputPath = "grading.txt";
        string outputPath = "report.txt";

        try
        {
            var processor = new StudentResultProcessor();

            // Read students
            var students = processor.ReadStudentsFromFile(inputPath);

            // Write report
            processor.WriteReportToFile(students, outputPath);

            Console.WriteLine("Report generated successfully in 'report.txt'.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: Input file not found.");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred: " + ex.Message);
        }
    }
}


