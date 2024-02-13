import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoaderComponent } from './loader.component';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Injectable({ providedIn: 'root' })
export class LoaderService {
  private waitingList: Observable<any>[] = [];
  private addWaiting = new Subject<Observable<any>>();
  private removeWaiting = new Subject<Observable<any> | undefined>();

  private current?: MatDialogRef<LoaderComponent>;
  constructor(private dialog: MatDialog) {
    this.addWaiting.pipe(untilDestroyed(this)).subscribe((loading) => {
      this.waitingList.push(loading);
      if (!this.current) {
        this.current = this.dialog.open(LoaderComponent, { disableClose: true });
      }
    });

    this.removeWaiting.pipe(untilDestroyed(this)).subscribe((loading) => {
      if (!loading) {
        this.waitingList = [];
      } else {
        const index = this.waitingList.indexOf(loading);
        if (index >= 0) {
          this.waitingList.splice(index, 1);
        }
      }

      if (!this.waitingList.length) {
        this.current?.close();
        this.current = undefined;
      }
    });
  }

  openForLoading<TResult>(loading: Observable<TResult>): Observable<TResult> {
    this.addWaiting.next(loading);
    return loading.pipe(tap((_) => this.removeWaiting.next(loading)));
  }

  closeLoader(): void {
    this.removeWaiting.next(undefined);
  }
}
