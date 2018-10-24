import { Injectable } from '@angular/core';
import { ClockWorker } from '../classes/clock-worker';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { ClockWorkerShort } from '../classes/clock-worker-short';
import { fromEvent } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WorkerService {
  constructor(private _httpClient: HttpClient) { }
  public getWorkers(): Promise<ClockWorkerShort[]> {
    return Promise.resolve([{ name: 'Max Mustermann', id: 1 }]);
    return this
    ._httpClient
    .get(environment.baseUri + '/api/worker')
    .pipe(map((p: ClockWorkerShort[]) => p))
    .toPromise();
  }

  public getWorker(id: number): Promise<ClockWorker> {
    return this
    ._httpClient
    .get(environment.baseUri + '/api/worker/' + id)
    .pipe(map((p: ClockWorker) => p))
    .toPromise();
  }
}
