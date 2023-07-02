import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-timeline',
  templateUrl: './time-line.component.html',
  styleUrls: ['./time-line.component.css']
})
export class TimeLineComponent implements OnInit {
@Input() value; 
@Input() success;
@Input() status;
@Input() temManageIntervent;
  constructor() { }
  ngOnInit(): void {
   
  }
}
