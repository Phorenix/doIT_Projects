import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ILog } from './log';
import { LogService } from './log.service';

@Component({
  selector: 'app-log-customer',
  templateUrl: './log-customer.component.html',
  styleUrls: ['./log-customer.component.css']
})
export class LogCustomerComponent implements OnInit {
  logs: ILog[];

  constructor(private logService: LogService, private route: ActivatedRoute) { }

  ngOnInit() {
    let id = +this.route.snapshot.paramMap.get('id');
    this.logService.getLogsById(id).subscribe(
      logs => this.logs = logs
    );
  }
}
