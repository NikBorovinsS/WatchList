import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Router, ActivatedRoute, Event } from '@angular/router';
import { TitlesService } from '../../services/titles.service';

@Component({
    selector: 'watchlist',
    templateUrl: './watchlist.component.html'
})

export class WatchlistComponent {
    public titleList: TitleData[] = [];
    public statusOptions: statusData[] = [{ status: 0, name: 'Planned to watch' }, { status: 1, name: 'Watched' }];
    //public statuses: any;

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

    delete(titleID) {
        var ans = confirm("Do you want to delete title with Id: " + titleID);
        if (ans) {
            this._titlesService.deleteTitle(titleID).subscribe((data) => {
                this.getTitles();
            }, error => console.error(error)) 
        }
    }
    statusSelected(statusCode: number, id: number) {
        console.log(statusCode + " " + id);
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
interface UIElement {
    addLoadListener(onload: (this: void, e: Event) => void): void;
}