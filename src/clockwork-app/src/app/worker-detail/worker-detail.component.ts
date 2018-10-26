import { Component, OnInit } from '@angular/core';
import { WorkerService } from 'src/shared/services/worker.service';

@Component({
  selector: 'app-worker-detail',
  templateUrl: './worker-detail.component.html',
  styleUrls: ['./worker-detail.component.scss']
})
export class WorkerDetailComponent implements OnInit {

  constructor(private _workerService: WorkerService, ) { }

  ngOnInit() {
    this._workerService.getWorker()
  }

}
