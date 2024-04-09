import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComboBoxModule } from '@syncfusion/ej2-angular-dropdowns';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { IgxComboModule, IgxSimpleComboModule } from "igniteui-angular";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListBrandComponent } from './brand/list-brand/list-brand.component';
import { PaginationComponent } from './pagination/pagination.component';
import { ToastrModule } from 'ngx-toastr';
import { CategoryComponent } from './category/category.component';
import { FeatureComponent } from './feature/feature.component';
import { RolesComponent } from './roles/roles.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    ListBrandComponent,
    PaginationComponent,
    CategoryComponent,
    FeatureComponent,
    RolesComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule, FormsModule, ReactiveFormsModule, ComboBoxModule, BrowserAnimationsModule,
    HttpClientModule,
    CommonModule,IgxComboModule, IgxSimpleComboModule,
    ToastrModule.forRoot({timeOut: 3000, positionClass: 'toast-top-right',})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
