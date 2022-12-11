using System.Diagnostics;

public class Program
{
    //Load a frequency dictionary or create a frequency dictionary from a text corpus
    public static void Main(string[] args)
    {
        
        string UnicodeString = "\u6500";
        
        var tes = " گزارش زومیت، اپل در بیانیه‌ای مطبوعاتی می‌گوید نسل جدید آیپد پرو محصولی همه‌کاره و قابل حمل با قدرت پردازشی بسیار زیاد است و به‌لطف بهره‌مندی از قلم لمسی اپل پنسل، تجربه‌ی لذت‌بخشی به کاربران ارائه می‌دهد. به ادعای اپل، آیپد پرو 2022 پیشرفته‌ترین صفحه‌نمایش را در بین دستگاه‌های قابل حمل دارد و با سیستم صوتی متشکل‌از چهار اسپیکر، تجربه‌ی شنیداری جذابی ارائه می‌دهد.".Split((char)8204);
        Console.WriteLine(UnicodeString);
        
        // for (int i = 0; i < tes.Length; i++)
        // {
        //     //Convert one by one every leter from the string in ASCII value.
        //     int value = tes[i];
        //     Console.WriteLine(" {0},{1}, {2}",i,value, tes[i]);
        // }
        //
        //set parameters
        const int initialCapacity = 82765;
        const int maxEditDistance = 2;
        const int prefixLength = 7;
        SymSpell symSpell = new SymSpell(initialCapacity, maxEditDistance, prefixLength);

        Console.Write("Creating dictionary ...");
        long memSize = GC.GetTotalMemory(true);
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        //Load a frequency dictionary
        //wordfrequency_en.txt  ensures high correction quality by combining two data sources: 
        //Google Books Ngram data  provides representative word frequencies (but contains many entries with spelling errors)  
        //SCOWL — Spell Checker Oriented Word Lists which ensures genuine English vocabulary (but contained no word frequencies)
        string path =
            @"C:\Users\mshaf\RiderProjects\SpellChecker\SpellChecker\fa_full.txt";
        //string path = "../../frequency_dictionary_en_82_765.txt";  //path when using symspell nuget package (frequency_dictionary_en_82_765.txt is included in nuget package)
        if (!symSpell.LoadDictionary(path, 0, 1))
        {
            Console.Error.WriteLine("\rFile not found: " + Path.GetFullPath(path));
            Console.ReadKey();
            return;
        }

        //lookup suggestions for single-word input strings
        string inputTerm="اطلاعاثی درمورد نسخه‌ی جهانی سری ردمی نوت ۱۲ به‌اشتراگ گذاشته است. براساس اعلام این منبع، سری نوت ۱۲ در سه‌ماهه‌ی اول ۲۰۲۳ در بازارهای جهانی عرضه خواهند شد. این درجالی است که سری ردمی نوت ۱۱ نیز ماه اکتبر سال گذسته در چین عرضه شد و حدود سه‌ماه بعد شاهد فروش نسخه‌ی جهانی این گوشی‌ها بودیم. بدین‌ترتیب میتوان انتظار داشت که این روند درمورد سری نوت ۱۲ نیز تکرار شود و نسخه‌ی جهانی این گوشی‌ها در ماه ژانویه یا فوریه دردسترس مشتریانقرار گیرند.";
        
        inputTerm="میکروچیپ";
        // string inputTerm = "مغاله";
        int maxEditDistanceLookup = 1; //max edit distance per lookup (maxEditDistanceLookup<=maxEditDistanceDictionary)
        var suggestionVerbosity = SymSpell.Verbosity.Closest; //T   op, Closest, All
        var suggestions = symSpell.Lookup(inputTerm, suggestionVerbosity, maxEditDistanceLookup);
        
        inputTerm="میکروچیپ";
        var maxEditDistance2 = 0;
       var suggestion1 = symSpell.WordSegmentation(inputTerm, maxEditDistance2);

//display suggestions, edit distance and term frequency
        foreach (var suggestion in suggestions)
        { 
            Console.WriteLine(suggestion.term +" "+ suggestion.distance.ToString() +" "+ suggestion.count.ToString("N0"));
        }


        //Alternatively Create the dictionary from a text corpus (e.g. http://norvig.com/big.txt ) 
        //Make sure the corpus does not contain spelling errors, invalid terms and the word frequency is representative to increase the precision of the spelling correction.
        //The dictionary may contain vocabulary from different languages. 
        //If you use mixed vocabulary use the language parameter in Correct() and CreateDictionary() accordingly.
        //You may use SymSpellCompound.CreateDictionaryEntry() to update a (self learning) dictionary incrementally
        //To extend spelling correction beyond single words to phrases (e.g. correcting "unitedkingom" to "united kingdom") simply add those phrases with CreateDictionaryEntry().
        //string path = "big.txt"
        //if (!SymSpellCompound.CreateDictionary(path,"")) Console.Error.WriteLine("File not found: " + Path.GetFullPath(path));

        stopWatch.Stop();
        long memDelta = GC.GetTotalMemory(true) - memSize;
        Console.WriteLine("\rDictionary: " + symSpell.WordCount.ToString("N0") + " words, "
                          + symSpell.EntryCount.ToString("N0") + " entries, edit distance=" +
                          symSpell.MaxDictionaryEditDistance.ToString()
                          + " in " + stopWatch.Elapsed.TotalMilliseconds.ToString("0.0") + "ms "
                          + (memDelta / 1024 / 1024.0).ToString("N0") + " MB");

        inputTerm="اطلاعاثی درمورد نسخه‌ی جهانی سری ردمی نوت ۱۲ به‌اشتراگ گذاشته است. براساس اعلام این منبع، سری نوت ۱۲ در سه‌ماهه‌ی اول ۲۰۲۳ در بازارهای جهانی عرضه خواهند شد. این درجالی است که سری ردمی نوت ۱۱ نیز ماه اکتبر سال گذسته در چین عرضه شد و حدود سه‌ماه بعد شاهد فروش نسخه‌ی جهانی این گوشی‌ها بودیم. بدین‌ترتیب میتوان انتظار داشت که این روند درمورد سری نوت ۱۲ نیز تکرار شود و نسخه‌ی جهانی این گوشی‌ها در ماه ژانویه یا فوریه دردسترس مشتریانقرار گیرند.";
        inputTerm="میکروچیپ";
        //warm up
        var result = symSpell.LookupCompound(inputTerm);

        foreach (var suggestion in result)
        {
            Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString() + " " +
                              suggestion.count.ToString("N0"));
        }
        
        string input;
        Console.WriteLine("Type in a word or phrase and hit enter to get suggestions:");
        while (!string.IsNullOrEmpty(input = (Console.ReadLine() ?? "").Trim()))
        {
            Correct(input, symSpell);
        }
    }

    private static void Correct(string input, SymSpell symSpell)
    {
        List<SymSpell.SuggestItem> suggestions = null;

        //check if input term or similar terms within edit-distance are in dictionary, return results sorted by ascending edit distance, then by descending word frequency     
        suggestions = symSpell.LookupCompound(input);

        //display term and frequency
        foreach (var suggestion in suggestions)
        {
            Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString() + " " +
                              suggestion.count.ToString("N0"));
        }
    }
}