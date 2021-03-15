# AwkSharp
This is a language I've decided to develop:


1.) because it's fun


2.) because I've always wanted a language that specializes in variable flexibility. 

# What's special about AwkSharp?
I'll be adding more here as I create, but here are some of the things that I enjoy about Awk#

- 'Destroy' keyword: this basically makes the variable specified disappear, allowing you to create a new variable of the same name and different type right after! This could
be useful in many ways, but a key one is just overall flexibility you can't get in most other languages

- Lock & Unlock keywords: These do exactly what they say they do. If you "lock" a variable, any attempts to assign a new value to it will leave it unchanged, 
it will essentially be read only until you pass the unlock keyword.

- All variables are global, meaning they can be accessed from anywhere. However, there is a local keyword that automatically destroys it at the end of the block

- I will be adding more as I go, but for now, that's it. I'm still trying to make it work :D

# Code Samples!

- IF statements:


```cs
int x = 1;
lock x;
IF x equ 1 {
    print "true";
} EL {
    print "false"
}
```
