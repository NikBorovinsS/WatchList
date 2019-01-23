import { Component, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { NgForm, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { WatchlistComponent } from '../watchlist/watchlist.component';
import { TitlesService } from '../../services/titles.service';

@Component({
    selector: 'edittitle',
    templateUrl: './edittitle.component.html'
})

export class edittitle implements OnInit {
    titleForm: FormGroup;
    title: string = "Edit";
    id: number;
    errorMessage: any;

    constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
        private _titlesService: TitlesService, private _router: Router) {
        if (this._avRoute.snapshot.params["id"]) {
            this.id = this._avRoute.snapshot.params["id"];
        }

        this.titleForm = this._fb.group({
            id: 0,
            name: ['', [Validators.required]],
            director: ['', [Validators.required]],
            description: ['', [Validators.required]],
            status: ['', [Validators.required]],
            score: ['', [Validators.required]],
            imdbu: ['', [Validators.required]],
            imdbr: ['', [Validators.required]],
        })
    }


    ngOnInit() {
        if (this.id > 0) {
            this.title = "Edit";
            this._titlesService.getTitleById(this.id)
                .subscribe(resp => this.titleForm.setValue(resp)
                    , error => this.errorMessage = error);
        }
    }

    save() {

        if (!this.titleForm.valid) {
            return;
        }

        if (this.title == "Create") {
            this._titlesService.saveTitle(this.titleForm.value)
                .subscribe((data) => {
                    this._router.navigate(['/fetch-title']);
                }, error => this.errorMessage = error)
        }
        else if (this.title == "Edit") {
            this._titlesService.updateTitle(this.titleForm.value)
                .subscribe((data) => {
                    this._router.navigate(['/fetch-title']);
                }, error => this.errorMessage = error)
        }
    }

    cancel() {
        this._router.navigate(['/fetch-title']);
    }

    get name() { return this.titleForm.get('name'); }
    get director() { return this.titleForm.get('director'); }
    get description() { return this.titleForm.get('description'); }
    get status() { return this.titleForm.get('status'); }
    get score() { return this.titleForm.get('score'); }
    get imdbu() { return this.titleForm.get('imdbu'); }
    get imdbr() { return this.titleForm.get('imdbr'); }
}



