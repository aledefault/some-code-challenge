# **Server Engineering Code Challenge**

## **Overview**
This challenge involved building a console-based word game inspired by slot machines. Players form words using letters from rotating reels, earning points based on letter values. The game uses a **Trie data structure** for fast word lookup.

## **Requirements**
1. **Game Logic:**
   - Implement a slot-machine-style word game.
   - Players form words using visible letters from six rotating reels.
   - Used letters move to the top of the reel, cycling the next letters downward.

2. **Trie Data Structure:**
   - Implement insert, search, and delete functions.
   - Populate the Trie with words from a dictionary file.

3. **File Handling:**
   - Load reels from `reels.txt`.
   - Load letter scores from `scores.txt`.
   - Validate words using `american-english-large.txt`.

4. **Gameplay:**
   - Show the player available letters.
   - Validate input against the Trie and reel constraints.
   - Award points based on letter values.
   - Update the reels dynamically after a valid word submission.

5. **Testing:**
   - Implement unit tests for all core functionality.
   - Ensure provided tests for the Trie pass.

## **Evaluation Criteria**
- Correctness of the game logic.
- Efficiency of the Trie implementation.
- Code cleanliness and organization.
- Robustness of testing.

## **Considerations about the solution**:
* The use of accents marks or not is up to you. To enable or disable them, just do it on StringExtensions.NormalizeFormat.
* I have implemented Trie in two ways; the one that was requested but also another one (**Hash base**) that beats the general tree data structure in all aspects except in space complexity (including simplicity). The behaviour can be switched, changing the generic parameter in `WithWordStorage` call, from `Trie` to `HashTrie`, in [Program.cs](/Program/ReelWords/Program.cs).

## **Run it**
```cmd
dotnet run --configuration Release
```

## **Benchmark**
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3737/23H2/2023Update/SunValley3)

AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores

### **Insert**
| Method          | Mean      | Error    | StdDev   | Ratio | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|---------------- |----------:|---------:|---------:|------:|-----------:|-----------:|----------:|----------:|------------:|
| InsertingInTrie | 283.33 ms | 3.071 ms | 2.873 ms |  1.00 | 14000.0000 | 13000.0000 | 1000.0000 |  219.3 MB |        1.00 |
| InsertingInHash |  66.87 ms | 1.296 ms | 1.774 ms |  0.24 |  4000.0000 |  2000.0000 | 1000.0000 |  60.45 MB |        0.28 |

### **Search**
| Method          | RepeatAllSearches | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0       | Allocated | Alloc Ratio |
|---------------- |------------------ |---------:|--------:|--------:|------:|--------:|-----------:|----------:|------------:|
| SearchingInTrie | 3                 | 175.4 ms | 1.35 ms | 1.20 ms |  1.00 |    0.00 | 20666.6667 | 334.28 MB |        1.00 |
| SearchingInHash | 3                 | 132.8 ms | 2.65 ms | 3.05 ms |  0.76 |    0.02 |  9000.0000 | 145.52 MB |        0.44 |

### **Delete**
| Method         | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0      | Gen1      | Allocated | Alloc Ratio |
|--------------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|----------:|------------:|
| DeletingInTrie | 66.80 ms | 1.014 ms | 0.899 ms |  1.00 |    0.00 | 6000.0000 | 1000.0000 |  111.3 MB |        1.00 |
| DeletingInHash | 36.86 ms | 0.721 ms | 0.912 ms |  0.55 |    0.02 | 3000.0000 | 1000.0000 |  48.51 MB |        0.44 |

### **Space complexity (Not a precise method)**
Trie:

* GC.GetTotalMemory()	1380272 - GC.GetTotalMemory()	11163000
* 8.67 MB Aprox

Hash:

* GC.GetTotalMemory()	1379512 - GC.GetTotalMemory()	16047192 
* 14.66 MB Aprox