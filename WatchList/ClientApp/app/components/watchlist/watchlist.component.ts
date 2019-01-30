import { Component, Inject, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Router, ActivatedRoute, Event } from '@angular/router';
import { TitlesService } from '../../services/titles.service';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'watchlist',
    templateUrl: './watchlist.component.html'
})

export class WatchlistComponent{
    public titleList: TitleData[] = [];
    selectedvalue = null;
    public statusOptions: statusData[] = [{ status: 0, name: 'Planned to watch' }, { status: 1, name: 'Watched' }];
    public statuses: any;

    constructor(public http: Http, private _router: Router, private _titlesService: TitlesService) {

        this.getTitles();
    }

    getTitles() {
        this._titlesService.getTitles().subscribe(
            data => this.titleList = data
        )
    }

    //OnLoad() {
    //    this.statuses = new Array(this.titleList.length);
    //    for (let i = 0; i < this.titleList.length; i++) {
    //        let newStatus = this.titleList[i].status;
    //        this.statuses.push(newStatus);
    //    }
    //}

    OnStatusChange(elem: any, newStatus:number) {
        
        elem.status = newStatus;

        this._titlesService.updateTitle(elem)
            .subscribe((data) => {
                this._router.navigate(['/fetch-title']);
            }, error => console.error(error))
    }

    OnScoreChange(elem: any, newScore: number) {

        elem.score = newScore;

        this._titlesService.updateTitle(elem)
            .subscribe((data) => {
                this._router.navigate(['/fetch-title']);
            }, error => console.error(error))
    }

    delete(titleID) {
        var ans = confirm("Do you want to delete title with Id: " + titleID);
        if (ans) {
            this._titlesService.deleteTitle(titleID).subscribe((data) => {
                this.getTitles();
            }, error => console.error(error)) 
        }
    }
}
interface TitleData {
    id: number;
    name: string;
    director: string;
    status: number;
    score: number;
    imdbu: string;
    imdbr: string;
    imgu: string;
    titletype: number;
    watchprogress: number;
    episodes: number;
    notes: string;
}

interface statusData {
    status: number;
    name: string;
}