import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';

import { MonitoringBoard } from './monitoringBoard/monitoringBoard.component';

const routes: Routes = [
  { path: 'monitoringBoard', component: MonitoringBoard },
  { path: '', redirectTo: 'monitoringBoard', pathMatch: 'full' }
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
  ],
})
export class AppRoutingModule { }
