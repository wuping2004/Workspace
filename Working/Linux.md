# Linux Command

- Grep file with case insensitive 
```
cat textfile.txt | grep -i “Hello”
```
- search recursively to read all files under each directory
```
grep -r "192.168.1.5" /etc/
```

- Search world only
```
grep -w "boo" file
```


Linux grep command options	Description
- -i	Ignore case distinctions on Linux and Unix
- -w	Force PATTERN to match only whole words
- -v	Select non-matching lines
- -n	Print line number with output lines
- -h	Suppress the Unix file name prefix on output
- -r	Search directories recursivly on Linux
- -R	Just like -r but follow all symlinks
- -l	Print only names of FILEs with selected lines
- -c	Print only a count of selected lines per FILE
- --color	Display matched pattern in colors