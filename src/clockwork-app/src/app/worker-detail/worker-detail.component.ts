import { Component, OnInit } from '@angular/core';
import { WorkerService } from 'src/shared/services/worker.service';
import { ClockWorker } from 'src/shared/classes/clock-worker';

@Component({
  selector: 'app-worker-detail',
  templateUrl: './worker-detail.component.html',
  styleUrls: ['./worker-detail.component.scss']
})
export class WorkerDetailComponent implements OnInit {
  public worker: ClockWorker;
  public selectedDate: Date;

  public beginWork: number;
  public endWork: number;
  public pause: number;

  public workSaved: boolean;

  constructor(private _workerService: WorkerService) { }

  ngOnInit() {
    this._workerService.getWorker(1).then(p => this.worker = p);
  }

  public addWork(): void {
    this._workerService.addWork(this.beginWork, this.endWork, this.pause).then(() => this.workSaved = true);
  }

}
