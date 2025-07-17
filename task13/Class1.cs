namespace task13;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.IO;

public class Subject
{
    public string Name { get; set; } = string.Empty;
    public int Grade { get; set; }

    public Subject() { }

    public Subject(string name, int grade)
    {
        Name = name;
        Grade = grade;
    }
}

public class Student
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    [JsonConverter(typeof(DateConv))]
    public DateTime BirthDate { get; set; }
    
    public List<Subject>? Grades { get; set; }
}

public class DateConv : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}

public static class JsonProcessing
{
    public static string ToJson(Student student)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        return JsonSerializer.Serialize(student, options);
    }

    public static Student FromJson(string json)
    {
        var options = new JsonSerializerOptions();
        var student = JsonSerializer.Deserialize<Student>(json, options);

        if (student.FirstName == null)
        {
            throw new Exception("FirstName required");
        }
        if (student.LastName == null)
        {
            throw new Exception("LastName required");
        }
        return student;
    }

    public static void SaveToFile(string filePath, Student student)
    {
        var json = JsonSerializer.Serialize(student);
        File.WriteAllText(filePath, json);
    }

    public static Student LoadFromFile(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Student>(json);
    }
}
