import { Component, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { TitlesService } from '../../services/titles.service'

@Component({
    selector: 'watchlist',
    templateUrl: './watchlist.component.html'
})

export class WatchlistComponent {
    public titleList: TitleData[] = [];

    constructor(public http: Http, private _router: Router, private _titlesService: TitlesService) {
        this.getTitles();
    }

    getTitles() {
        this._titlesService.getTitles().subscribe(
            data => this.titleList = data
        )
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
    description: string;
    status: number;
    score: number;
    imdbu: string;
    imdbr: string;
    }