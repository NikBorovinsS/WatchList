import { NgModule } from '@angular/core';
import { TitlesService } from './services/titles.service'
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { WatchlistComponent } from './components/watchlist/watchlist.component'
import { createtitle } from './components/addtitle/AddTitle.component'
import { edittitle } from './components/edittitle/edittitle.component'

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        WatchlistComponent,
        createtitle,
        edittitle,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'fetch-title', component: WatchlistComponent },
            { path: 'register-title', component: createtitle },
            { path: 'watchlist/edit/:id', component: edittitle },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [TitlesService]
})
export class AppModuleShared {
}
