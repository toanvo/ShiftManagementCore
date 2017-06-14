import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { Employee } from "models/employee";

@Injectable()
export class EmployeeService {
    constructor(private http: Http)
    { }

    private baseUrl = "api/employee/";  

    get(id: number)
    {
        if (id == null)
        {
            throw new Error("The employee id is required.");
        }

        var url = this.baseUrl + id;
        return this.http.get(url).map(res => <Employee>res.json()).catch(this.handleError);
    }

    private handleError(error: Response)
    {
        // output errors to the console.        
        console.error(error);
        return Observable.throw(error.json().error || "Server error");
    }
}


