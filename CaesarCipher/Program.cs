using System.Reflection.Metadata.Ecma335;

Dictionary<char, int> numberReferences = new Dictionary<char, int>();
char[] letters = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
bool stillRunning = true;

AddDictionaryElements();

while (stillRunning) 
{
    string plainText = GetUserString();
    int key = GetKeyInput();
    string cipherText = EncryptPlainText(plainText, key);

    Console.WriteLine($"Plaintext: {plainText}");
    Console.WriteLine($"Ciphertext: {cipherText}");
    Console.WriteLine($"Shift Value: {key}");

    stillRunning = AskIfStillRunning(); 
}
Console.WriteLine("Thank you for using this software!");

void AddDictionaryElements()
{
    for (int i = 0; i < 26; i++)
    {
        numberReferences.Add(letters[i], i);
    }
}

string GetUserString()
{
    bool validInput = false;
    string? userInput = string.Empty;

    Console.WriteLine("Please enter a string to encrypt (no numbers or symbols allowed): ");
    while (!validInput)
    {
        Console.Write(">> ");
        userInput = Console.ReadLine();
        if (userInput is not null && userInput != string.Empty)
        {
            bool contentsValid = ValidateStringContents(userInput);
            if (contentsValid)
            {
                Console.WriteLine("Input accepted!");
                validInput = true;
            }
        }
        else
        {
            Console.WriteLine("Input declined: String either null or empty.");
        }
    }
    if (userInput is not null)
    {
        return userInput;
    }
    else
    {
        return string.Empty;
    }
        
}

int GetKeyInput()
{
    string? userInput = string.Empty;
    bool validInput = false;
    int userIntInput = 0;

    Console.WriteLine("Please enter the shift key for the cipher: ");
    while (!validInput)
    {
        Console.Write(">> ");
        userInput = Console.ReadLine();
        if (int.TryParse(userInput, out userIntInput))
        {
            if (userIntInput > int.MinValue && userIntInput < int.MaxValue)
            {
                Console.WriteLine("Input accepted!");
                validInput = true;
            }
            else
            {
                Console.WriteLine("Input declined: integer out of bounds.");
            }
        }
        else
        {
            Console.WriteLine("Input declined: input cannot parse to an integer.");
        }
    }
    return userIntInput;
}

bool ValidateStringContents(string stringInput)
{
    bool invalidCharFlag = false;
    foreach (char c in stringInput.ToLower().Trim())
    {
        if (c == ' ')
        {
            continue;
        }
        if (!letters.Contains(c))
        {
            invalidCharFlag = true;
            break;
        }
    }
    if (invalidCharFlag)
    {
        Console.WriteLine("Input declined: Invalid character(s) dected in string.");
        return false;
    }
    else
    {
        return true;
    }
}

string EncryptPlainText(string plainText, int key)
{
    string cipherText = string.Empty;
    int?[] charNumericals = new int?[plainText.Length];
    int indexPointer = 0;

    foreach (char c in plainText.ToLower().Trim())
    {
        if (c == ' ')
        {
            charNumericals[indexPointer++] = null;
            continue;
        }
        else
        {
            int? charValue = numberReferences[c];
            charValue += key;
            if (charValue >= 0)
            {
                charValue %= 26;
            }
            else
            {
                while (charValue < 0)
                {
                    charValue += 26;
                }
            }
            charNumericals[indexPointer++] = charValue;
        }
    }

    foreach (int? charNum in charNumericals)
    {
        if (charNum is null)
        {
            continue;
        }
        else 
        {
            cipherText += letters[(int)charNum];
        }    
    }
    return cipherText;
}

bool AskIfStillRunning() 
{
    bool running = true;
    bool validInput = false;
    string? userInput = string.Empty;

    Console.WriteLine("Do you want to encrypt another string? (y/n)");
    while (!validInput)
    {
        Console.Write(">> ");
        userInput = Console.ReadLine();

        if (userInput is not null && userInput != string.Empty)
        {
            if (userInput.Length == 1)
            {
                switch (userInput) 
                {
                    case "y":
                        validInput = true;
                        running = true;
                        break;
                    case "n":
                        validInput = true;
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Error: Input invalid (y/n)");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Error: Input too long.");
            }

        }
        else 
        {
            Console.WriteLine("Error: Input is null or empty.");
        }
    }
    return running;
}