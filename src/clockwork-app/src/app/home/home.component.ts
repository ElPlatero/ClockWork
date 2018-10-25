import { Component, OnInit } from '@angular/core';
import { WorkerService } from '../../shared/services/worker.service';
import { ClockWorkerShort } from '../../shared/classes/clock-worker-short';
import { ClockWorker } from 'src/shared/classes/clock-worker';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private workers: ClockWorkerShort[];
  private newWorker: ClockWorker;

  constructor(private _workerService: WorkerService) {
    this.newWorker = new ClockWorker();
  }

  ngOnInit() {
    this._workerService.getWorkers().then(p => {
      this.workers = p;
      console.log(p);
    }).catch((err) => {
      console.error(err);
    });
  }

}
