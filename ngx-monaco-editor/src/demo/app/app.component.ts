import { Component } from '@angular/core';
import {CompileService} from "./compile.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  exNumber: number;
  themes = ['vs', 'vs-dark', 'hc-black'];
  theme = 'vs-dark';
  languages = ['java', 'csharp'];
  language = 'java';
  options = { theme: 'vs-dark', readOnly: false, language: 'java' };
  outputs: string[];
  codePlaceHolders =
    {
      csharp: `using System;
namespace MyApp
{
    class Program
    {
      static void Main(string[] args)
      {
        Console.WriteLine("Hello, world!");
      }
    }
}`,
      java: `import java.util.Scanner;
class Main {
    public static void main(String[] args) {
        System.out.println("Hello World!");
    }
}`
    };
  exercise1 = {
    csharp: `//Viet ham nhan doi
using System;
namespace MyApp
{
    class Program
    {
        public static int nhandoi(int a) {
            //Viet ham tai day
            return a;
        }
      static void Main(string[] args)
      {
        Console.WriteLine(nhandoi(Convert.ToInt32(args[0])));
      }
    }
}`,
    java: `//hay viet ham nhan doi
import java.util.Scanner;
import java.lang.Math;
class Main {
    public static int nhandoi(int a) {
        //viet ham nhan doi tai day
        return a;
    }
    public static void main(String[] args) {
        System.out.println(nhandoi(Integer.parseInt(args[0])));
    }
}`
  }
  exercise2 = {
    csharp: `//Viet ham tinh binh phuong so nguyen
using System;
namespace MyApp
{
    class Program
    {
        public static int binhphuong(int a) {
            //Viet ham tai day
            return a;
        }
      static void Main(string[] args)
      {
        Console.WriteLine(binhphuong(Convert.ToInt32(args[0])));
      }
    }
}`,
    java: `//hay viet ham tinh binh phuong
import java.util.Scanner;
import java.lang.Math;
class Main {
    public static int binhphuong(int a) {
        //viet ham tai day
        return a;
    }
    public static void main(String[] args) {
        System.out.println(binhphuong(Integer.parseInt(args[0])));
    }
}`
  }

  constructor(private compileService: CompileService){}

  codeText = this.codePlaceHolders.java

  setTheme(theme) {
    this.options = {...this.options, theme}
  }

  setLanguage(language) {
    this.options = {...this.options, language};
    this.examine();
  }
  examine() {
    if(!this.exNumber) {
      return this.codeText = this.options.language == 'java' ? this.codePlaceHolders.java : this.codePlaceHolders.csharp;
    }
    if (this.exNumber ==1) {
      return this.codeText = this.options.language == 'java' ? this.exercise1.java : this.exercise1.csharp;
    }
    return this.codeText = this.options.language == 'java' ? this.exercise2.java : this.exercise2.csharp;
  }

  doExercise(exNumber: number) {
    this.exNumber = exNumber;
    this.examine();
  }

  normalCompile() {
    this.exNumber = null;
    this.examine();
  }

  compile() {
    if (!this.exNumber) {
      if(this.options.language == 'java'){
        return this.compileService.compileJava(this.codeText).subscribe(data => {
          this.outputs = data;
        }, () => alert("server đang không chạy hoặc bị lỗi"))
      }
      return this.compileService.compileCSharp(this.codeText).subscribe(data => this.outputs = data,
        () => alert("server đang không chạy hoặc bị lỗi"));
    }
    return this.compileService.checkExercise(this.exNumber, this.options.language, this.codeText).subscribe();
  }
}
