import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { ILog } from './log';

@Injectable({
  providedIn: 'root'
})
export class LogService {
  private logUrl = 'http://localhost:60734/api/logs';

  constructor(private http: HttpClient) {}

  getLogs(): Observable<ILog[]> {
    return this.http.get<ILog[]>(this.logUrl).pipe(
      tap(data => console.log("All: " + JSON.stringify(data)))
    );
  }

  getLogsById(id: number): Observable<ILog[]> {
    return this.http.get<ILog[]>(`${this.logUrl}/${id}`).pipe(
      tap(data => console.log("All: " + JSON.stringify(data)))
    );
  }
}
