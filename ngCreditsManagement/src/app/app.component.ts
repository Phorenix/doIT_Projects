import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  selectedLanguage: string = 'en';

  constructor(private translateService: TranslateService) { }

  public changeLaguage(): void {
    this.translateService.use(this.selectedLanguage);
  }

  ngOnInit(): void {
    this.translateService.setDefaultLang('en');
  }
}
