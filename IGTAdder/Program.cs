namespace IGTAdder;

internal class Program {
    static void Main(string[] args) {
        try {
            Start();
        }
        catch (Exception ex) {
            Console.WriteLine("A unexpected error occurred:");
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("Press ENTER to exit");
        Console.ReadLine();
    }

    static void Start() {
        Console.WriteLine("IGT Adder - Version 1.0.0");
        Console.WriteLine("Adds In Game Times with support for centiseconds and milliseconds");
        Console.WriteLine();
        try {
            Console.Title = "IGT Adder 1.0.0";
        }
        catch { }
        Console.WriteLine("Select choice:");
        Console.WriteLine("1. Read IGT data from file");
        Console.WriteLine("2. Input IGT data to this console");
        Console.WriteLine();
        int choiceInt;
        while (true) {
            Console.Write("Choice: ");
            var choice = Console.ReadLine();
            if (string.IsNullOrEmpty(choice)) return;
            if (!int.TryParse(choice, out choiceInt) || choiceInt <= 0 || choiceInt > 2) {
                Console.WriteLine("Not a valid choice");
                continue;
            }
            break;
        }
        Console.WriteLine("Select IGT format:");
        Console.WriteLine("1. 0'00 or 0:00");
        Console.WriteLine("2. 0'00''00 or 0:00:00 (centiseconds)");
        Console.WriteLine("3. 0'00''000 or 0:00:000 (milliseconds)");
        Console.WriteLine();
        int IGTFormat;
        while (true) {
            Console.Write("Choice: ");
            var choice = Console.ReadLine();
            if (string.IsNullOrEmpty(choice)) return;
            if (!int.TryParse(choice, out IGTFormat) || IGTFormat <= 0 || IGTFormat > 3) {
                Console.WriteLine("Not a valid choice");
                continue;
            }
            break;
        }
        List<string> IGTData;
        if (choiceInt == 1) {
            while (true) {
                Console.WriteLine("Input path to IGT file. Each time must be on its own line");
                Console.Write("Enter path (Leave empty to exit): ");
                string IGTFile = Console.ReadLine();
                if (string.IsNullOrEmpty(IGTFile)) return;
                if (!File.Exists(IGTFile)) {
                    Console.WriteLine("File does not exist");
                    continue;
                }
                IGTData = new(File.ReadAllLines(IGTFile));
                break;
            }
        }
        else if (choiceInt == 2) {
            IGTData = new();
            Console.WriteLine("When you have input all times hit ENTER with no time entered");
            while (true) {
                Console.Write("Enter time: ");
                string IGTTime = Console.ReadLine();
                if (string.IsNullOrEmpty(IGTTime)) break;
                IGTData.Add(IGTTime);
            }
        }
        else return;
        string timeStr = ParseTimes(IGTData, IGTFormat);
        if (timeStr == null) return;
        Console.WriteLine($"Total IGT: {timeStr}");
    }

    private static readonly char[] separator = ['\'', ':'];

    static string ParseTimes(List<string> times, int format) {
        switch (format) {
            case 1: {
                    //hh:mm:ss
                    uint totalHours = 0;
                    uint totalMinutes = 0;
                    uint totalSeconds = 0;
                    foreach (string time in times) {
                        string[] timeSplit = time.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        try {
                            if (timeSplit.Length is not 2 or 3) throw new FormatException();
                            if (timeSplit.Length == 2) {
                            totalMinutes += uint.Parse(timeSplit[0].Trim());
                                totalSeconds += uint.Parse(timeSplit[1].Trim());
                            }
                            else {
                                totalHours += uint.Parse(timeSplit[0].Trim());
                                totalMinutes += uint.Parse(timeSplit[1].Trim());
                                totalSeconds += uint.Parse(timeSplit[2].Trim());
                            }
                        }
                        catch (FormatException) {
                            Console.WriteLine($"Failed to parse {time} as a time string. Format must be 0'00 or 0:00");
                            return null;
                        }
                    }
                    while (totalSeconds >= 60) {
                        totalSeconds -= 60;
                        totalMinutes += 1;
                    }
                    while (totalMinutes >= 60) {
                        totalMinutes -= 60;
                        totalHours += 1;
                    }
                    if (totalHours == 0) return $"{totalMinutes:D2}:{totalSeconds:D2}";
                    return $"{totalHours:D2}:{totalMinutes:D2}:{totalSeconds:D2}";
                }
            case 2: {
                    //hh:mm:ss:cs
                    uint totalHours = 0;
                    uint totalMinutes = 0;
                    uint totalSeconds = 0;
                    uint totalCentiseconds = 0;
                    foreach (string time in times) {
                        string[] timeSplit = time.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        try {
                            if (timeSplit.Length is not 3 or 4) throw new FormatException();
                            if (timeSplit.Length == 3) {
                            totalMinutes += uint.Parse(timeSplit[0].Trim());
                                totalSeconds += uint.Parse(timeSplit[1].Trim());
                                totalCentiseconds += uint.Parse(timeSplit[2].Trim());
                            }
                            else {
                                totalHours += uint.Parse(timeSplit[0].Trim());
                                totalMinutes += uint.Parse(timeSplit[1].Trim());
                                totalSeconds += uint.Parse(timeSplit[2].Trim());
                                totalCentiseconds += uint.Parse(timeSplit[3].Trim());
                            }
                        }
                        catch (FormatException) {
                            Console.WriteLine($"Failed to parse {time} as a time string. Format must be 0'00''00 or 0:00:00");
                            return null;
                        }
                    }
                    while (totalCentiseconds >= 100) {
                        totalCentiseconds -= 100;
                        totalSeconds += 1;
                    }
                    while (totalSeconds >= 60) {
                        totalSeconds -= 60;
                        totalMinutes += 1;
                    }
                    while (totalMinutes >= 60) {
                        totalMinutes -= 60;
                        totalHours += 1;
                    }
                    if (totalHours == 0) return $"{totalMinutes:D2}:{totalSeconds:D2}:{totalCentiseconds:D2}";
                    return $"{totalHours:D2}:{totalMinutes:D2}:{totalSeconds:D2}:{totalCentiseconds:D2}";
                }
            case 3: {
                    //hh:mm:ss:ms
                    uint totalHours = 0;
                    uint totalMinutes = 0;
                    uint totalSeconds = 0;
                    uint totalMilliseconds = 0;
                    foreach (string time in times) {
                        string[] timeSplit = time.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        try {
                            if (timeSplit.Length is not 3 or 4) throw new FormatException();
                            if (timeSplit.Length == 3) {
                            totalMinutes += uint.Parse(timeSplit[0].Trim());
                                totalSeconds += uint.Parse(timeSplit[1].Trim());
                                totalMilliseconds += uint.Parse(timeSplit[2].Trim());
                            }
                            else {
                                totalHours += uint.Parse(timeSplit[0].Trim());
                                totalMinutes += uint.Parse(timeSplit[1].Trim());
                                totalSeconds += uint.Parse(timeSplit[2].Trim());
                                totalMilliseconds += uint.Parse(timeSplit[3].Trim());
                            }
                        }
                        catch (FormatException) {
                            Console.WriteLine($"Failed to parse {time} as a time string. Format must be 0'00''000 or 0:00:000");
                            return null;
                        }
                    }
                    while (totalMilliseconds >= 1000) {
                        totalMilliseconds -= 1000;
                        totalSeconds += 1;
                    }
                    while (totalSeconds >= 60) {
                        totalSeconds -= 60;
                        totalMinutes += 1;
                    }
                    while (totalMinutes >= 60) {
                        totalMinutes -= 60;
                        totalHours += 1;
                    }
                    if (totalHours == 0) return $"{totalMinutes:D2}:{totalSeconds:D2}:{totalMilliseconds:D2}";
                    return $"{totalHours:D2}:{totalMinutes:D2}:{totalSeconds:D2}:{totalMilliseconds:D3}";
                }
            default: return string.Empty;
        }
    }
}
