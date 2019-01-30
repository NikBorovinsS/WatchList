import { Component, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { WatchlistComponent } from '../watchlist/watchlist.component';
import { TitlesService } from '../../services/titles.service';

@Component({
    selector: 'createtitle',
    templateUrl: './AddTitle.component.html',
    styleUrls: ['./AddTitle.component.css']

})

export class createtitle implements OnInit {
    searchForm: FormGroup;
    title: string = "Create";
    titleForm: FormGroup;
    id: number = 0;
    errorMessage: any;
    processValidation = false;
    visibleTable = false; 
    allMovies: MovieData[] = [];

    constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
        private _titlesService: TitlesService, private _router: Router) {
        if (this._avRoute.snapshot.params["id"]) {
            this.id = this._avRoute.snapshot.params["id"];
        }

        this.searchForm = this._fb.group({
            id: 0,
            film: ['', [Validators.required]],
        })

        this.titleForm = this._fb.group({
            id: 0,
            name: ['', [Validators.required]],
            director: ['', [Validators.required]],
            status: ['', [Validators.required]],
            score: ['', [Validators.required]],
            imdbu: ['', [Validators.required]],
            imdbr: ['', [Validators.required]],
        })
    }

    addTitle(elem) {
        this._titlesService.addTitleFrom(elem)
            .subscribe((data) => {
                    this._router.navigate(['/fetch-title']);
                }, error => this.errorMessage = error);
     }

    onTitleFormSubmit() {
        this.processValidation = true;
        this.visibleTable = true;
        if (this.searchForm.invalid) {
            return; //Validation failed, exit from method.
        }
        //Form is valid, now perform create or update
        let movie = this.searchForm.controls['film'].value;

        //Generate article id then create article
        this._titlesService.getResultsBy(movie)
            .subscribe(
                data => this.allMovies = data
            );
    }


    ngOnInit() {
    }

    //save() {

    //    if (!this.titleForm.valid) {
    //        return;
    //    }

    //    if (this.title == "Create") {
    //        this._titlesService.saveTitle(this.titleForm.value)
    //            .subscribe((data) => {
    //                this._router.navigate(['/fetch-title']);
    //            }, error => this.errorMessage = error)
    //    }
    //    else if (this.title == "Edit") {
    //        this._titlesService.updateTitle(this.titleForm.value)
    //            .subscribe((data) => {
    //                this._router.navigate(['/fetch-title']);
    //            }, error => this.errorMessage = error)
    //    }
    //}

    cancel() {
        this._router.navigate(['/fetch-title']);
    }
}



interface MovieData {
    id: number;
    name: string;
    director: string;
    status: number;
    score: number;
    imdbu: string;
    imgu: string;
    imgup: string;
    tytletype: string;
    watchprogress: string;
    notes: string;
    episodes: string;
}