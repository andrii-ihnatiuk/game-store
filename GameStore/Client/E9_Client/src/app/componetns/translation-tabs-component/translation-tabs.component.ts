import { Component, ContentChild, Input, OnChanges, SimpleChanges, TemplateRef } from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { BehaviorSubject } from 'rxjs';
import { Culture } from 'src/app/models/culture.model';

@Component({
  selector: 'app-translation-tabs',
  templateUrl: './translation-tabs.component.html',
  styleUrls: ['./translation-tabs.component.scss']
})
export class TranslationTabsComponent implements OnChanges {

  @Input()
  cultures: Culture[] = [];

  defaultCultureIndex: number = 0;

  loadedTabs: boolean[] = [];

  @ContentChild(TemplateRef)
  templateRef!: TemplateRef<any>;

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    if (!!this.cultures && this.cultures.length > 0) {
      this.loadedTabs = [];

      this.cultures.forEach((culture, index) => {
        this.loadedTabs.push(false);
        if (culture.isDefault) {
          this.defaultCultureIndex = index;
          this.loadedTabs[index] = true;
        }
      })
    }
  }

  onTabChange(event: MatTabChangeEvent): void {
    this.loadedTabs[event.index] = true;
  }

}
