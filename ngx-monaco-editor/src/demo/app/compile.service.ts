import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import { map, catchError } from "rxjs/operators";
@Injectable({
  providedIn: 'root'
})
export class CompileService {

  protected rootUrl = "https://localhost:44302/api/compile"
  constructor(
    protected httpClient: HttpClient,
  ) { }

  public compileJava(codeText: string) {
    let url = this.rootUrl + "/java"
    return this.httpClient.post(url, {codeText}).pipe(
      map((data: any) => {
        console.log("successfully compile java...", data);
        return data;
        }));
  }

  public compileCSharp(codeText: string) {
    let url = this.rootUrl + "/csharp"
    return this.httpClient.post(url, {codeText}).pipe(
      map((data: any) => {
        console.log("successfully compile C#...", data);
        return data;
      }))
  }

  public checkExercise(exeNumber: number, language: string, codeText: string) {
    let url = this.rootUrl + `/exercise${exeNumber}`;
    return this.httpClient.post(url, {language, codeText}).pipe(
      map((data:boolean) => {
        if (data)
          alert("correct!");
        else
          alert("wrong answer, please try again");
      })
    )
  }
}
