
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string[] tasks = new string[100];
        int[] priorities = new int[100];
        string[] dates = new string[100];
        int count = 0;
        int choice;

        // Load saved tasks automatically on system start
        LoadTasksFromFile(tasks, priorities, dates, ref count);

        do
        {
            Console.WriteLine("\n=== SMART DAILY TASK LIST ===");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. View Top 3 Tasks");
            Console.WriteLine("4. Save Tasks to File");
            Console.WriteLine("5. Load Tasks from File");
            Console.WriteLine("6. Delete Task");
            Console.WriteLine("7. Exit");
            Console.Write("Enter choice: ");

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    if (count >= 100)
                    {
                        Console.WriteLine("Task list is full!");
                        break;
                    }

                    Console.Write("Enter task name: ");
                    tasks[count] = Console.ReadLine() ?? "";

                    Console.Write("Enter priority (1-5): ");

                    while (!int.TryParse(Console.ReadLine(), out priorities[count]) ||
                           priorities[count] < 1 || priorities[count] > 5)
                    {
                        Console.Write("Invalid! Enter priority (1-5): ");
                    }

                    Console.Write("Enter task date (MM/DD/YYYY): ");
                    dates[count] = Console.ReadLine() ?? "";

                    count++;

                    Console.WriteLine("Task added successfully!");
                    break;

                case 2:
                    if (count == 0)
                    {
                        Console.WriteLine("No tasks available.");
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Console.WriteLine($"\nTask {i + 1}");
                            Console.WriteLine($"Name: {tasks[i]}");
                            Console.WriteLine($"Priority: {priorities[i]}");
                            Console.WriteLine($"Date: {dates[i]}");
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("\n=== TOP 3 TASKS ===");

                    if (count == 0)
                    {
                        Console.WriteLine("No tasks available.");
                    }
                    else
                    {
                        bool[] shown = new bool[count];
                        //set how many the rank numbers are
                        for (int rank = 0; rank < 3 && rank < count; rank++)
                         {   int highest = -1;

                            for (int i = 0; i < count; i++)
                            {
                                if (!shown[i] &&
                                    (highest == -1 ||
                                     priorities[i] > priorities[highest]))
                                {
                                    highest = i;
                                }
                            }

                            Console.WriteLine($"\n{rank + 1}. {tasks[highest]}");
                            Console.WriteLine($"Priority: {priorities[highest]}");
                            Console.WriteLine($"Date: {dates[highest]}");

                            shown[highest] = true;
                        }
                    }
                    break;

                case 4:
                    SaveTasksToFile(tasks, priorities, dates, count);
                    break;

                case 5:
                    LoadTasksFromFile(tasks, priorities, dates, ref count);
                    break;

                case 6:
                    if (count == 0)
                    {
                        Console.WriteLine("No tasks available to delete.");
                        break;
                    }

                    Console.WriteLine("\n=== DELETE TASK ===");

                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {tasks[i]}");
                    }

                    Console.Write("Enter task number to delete: ");

                    if (int.TryParse(Console.ReadLine(), out int deleteIndex) &&
                        deleteIndex >= 1 && deleteIndex <= count)
                    {
                        deleteIndex--;

                        for (int i = deleteIndex; i < count - 1; i++)
                        {
                            tasks[i] = tasks[i + 1];
                            priorities[i] = priorities[i + 1];
                            dates[i] = dates[i + 1];
                        }

                        count--;

                        // Automatically save after deletion 
                        SaveTasksToFile(tasks, priorities, dates, count);

                        Console.WriteLine("Task deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid task number.");
                    }
                    break;

                case 7:
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

        } while (choice != 7);
    }

    // Save all tasks to a txt file 
    static void SaveTasksToFile(string[] tasks, int[] priorities,
                                string[] dates, int count)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("tasks.txt"))
            {
                for (int i = 0; i < count; i++)
                {
                    writer.WriteLine(
                        $"{tasks[i]}|{priorities[i]}|{dates[i]}");
                }
            }

            Console.WriteLine("Tasks saved to tasks.txt successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    // Load tasks from file into arrays
    static void LoadTasksFromFile(string[] tasks, int[] priorities,
                                  string[] dates, ref int count)
    {
        count = 0;

        if (!File.Exists("tasks.txt"))
        {
            Console.WriteLine("No saved task file found.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines("tasks.txt");

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('|');

                if (parts.Length >= 3 &&
                    int.TryParse(parts[1], out int p))
                {
                    if (count >= 100)
                        break;

                    tasks[count] = parts[0];
                    priorities[count] = p;
                    dates[count] = parts[2];

                    count++;
                }
            }

            Console.WriteLine($"{count} tasks loaded from tasks.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }
}
