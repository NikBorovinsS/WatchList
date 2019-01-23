import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class TitlesService {
    myAppUrl: string = "";

    constructor(private _http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.myAppUrl = baseUrl;
    }

    getTitles() {
        return this._http.get(this.myAppUrl + 'api/WatchList/Index')
            .map((response: Response) => response.json())
            .catch(this.errorHandler);
    }

    getResultsBy(name: string) {
        return this._http.get(this.myAppUrl + 'api/Watchlist/ResultsBy/' + name)
            .map((response: Response) => response.json())
            .catch(this.errorHandler);
    }

    getTitleById(id: number) {
        return this._http.get(this.myAppUrl + "api/WatchList/Details/" + id)
            .map((response: Response) => response.json())
            .catch(this.errorHandler)
    }

    saveTitle(title) {
        return this._http.post(this.myAppUrl + 'api/WatchList/Create', title)
            .map((response: Response) => response.json())
            .catch(this.errorHandler)
    }

    addTitleFrom(elem) {
        return this._http.post(this.myAppUrl + "api/WatchList/AddTitleFrom",elem)
            .map((response: Response) => {
                console.log(response.json());
                console.log(this.myAppUrl + "api/WatchList/AddTitleFrom" );
                response.json();
            })
           .catch(this.errorHandler)
    }

    updateTitle(title) {
        return this._http.put(this.myAppUrl + 'api/WatchList/Edit', title)
            .map((response: Response) => response.json())
            .catch(this.errorHandler);
    }

    deleteTitle(id) {
        return this._http.delete(this.myAppUrl + "api/WatchList/Delete/" + id)
            .map((response: Response) => response.json())
            .catch(this.errorHandler);
    }

    errorHandler(error: Response) {
        console.log(error);
        return Observable.throw(error);
    }
}