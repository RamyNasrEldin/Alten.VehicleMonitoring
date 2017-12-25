import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppRoutingModule } from './app.routing';
import { ComponentsModule } from './components/components.module';

import { AppComponent } from './app.component';

import { MonitoringBoard } from './monitoringBoard/monitoringBoard.component';
import { CustomerVehicles } from './customerVehicles/customerVehicles.component';
import { Filters } from './filters/filters.component'

import { MonitoringBoardService } from './services/monitoringBoard.service';

@NgModule({
  declarations: [
    AppComponent,
    MonitoringBoard,
    CustomerVehicles,
    Filters
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    ComponentsModule,
    RouterModule,
    AppRoutingModule
  ],
  providers: [MonitoringBoardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
