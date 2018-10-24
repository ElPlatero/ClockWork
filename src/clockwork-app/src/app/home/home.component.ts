import { Component, OnInit } from '@angular/core';
import { WorkerService } from '../../shared/services/worker.service';
import { ClockWorkerShort } from '../../shared/classes/clock-worker-short';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private workers: ClockWorkerShort[];

  constructor(private _workerService: WorkerService) { }

  ngOnInit() {
    this._workerService.getWorkers().then(p => {
      this.workers = p;
      console.log(p);
    }).catch((err) => {
      console.error(err);
    });
  }

}
