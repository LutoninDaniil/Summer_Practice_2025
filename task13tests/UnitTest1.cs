namespace task13tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using task13;

public class JsonSerializationTests
{
    [Fact]
    public void Serialize_Student_ReturnsValidJson()
    {
        var student = new Student
        {
            FirstName = "Den",
            LastName = "Oskar",
            BirthDate = new DateTime(2020, 12, 11),
            Grades = new List<Subject>
            {
                new Subject("Biology", 3),
                new Subject("German", 5)
            }
        };

        string json = JsonProcessing.ToJson(student);

        Assert.Contains("\"FirstName\": \"Den\"", json);
        Assert.Contains("\"LastName\": \"Oskar\"", json);
        Assert.Contains("\"BirthDate\": \"2020-12-11\"", json);
        Assert.Contains("\"Grades\"", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsStudent()
    {
        string json = @"{""FirstName"": ""Den"", ""LastName"": ""Oskar"", ""BirthDate"": ""2020-12-11"",
        ""Grades"": [{ ""Name"": ""Biology"", ""Grade"": 3 },{ ""Name"": ""German"", ""Grade"": 5 }]}";

        var student = JsonProcessing.FromJson(json);

        Assert.Equal("Den", student.FirstName);
        Assert.Equal("Oskar", student.LastName);
        Assert.Equal(new DateTime(2020, 12, 11), student.BirthDate);
        Assert.NotNull(student.Grades);
        Assert.Equal(2, student.Grades.Count);
    }

    [Fact]
    public void SaveAndLoad_Student_ReturnsSameData()
    {
        var student = new Student
        {
            FirstName = "Peter",
            LastName = "Petrovich",
            BirthDate = new DateTime(1987, 7, 21),
            Grades = new List<Subject>
            {
                new Subject("Astronomy ", 5)
            }
        };

        string filePath = Path.GetTempFileName();

        try
        {
            JsonProcessing.SaveToFile(filePath, student);
            var loadedStudent = JsonProcessing.LoadFromFile(filePath);

            Assert.Equal(student.FirstName, loadedStudent.FirstName);
            Assert.Equal(student.LastName, loadedStudent.LastName);
            Assert.Equal(student.BirthDate, loadedStudent.BirthDate);
            Assert.NotNull(student.Grades);
            Assert.Equal(student.Grades[0].Name, loadedStudent.Grades[0].Name);
            Assert.Equal(student.Grades[0].Grade, loadedStudent.Grades[0].Grade);
        }
        finally
        {
            File.Delete(filePath);
        }
    }
}
