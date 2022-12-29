import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SystemFile } from '../../_models/SystemFile';

@Component({
  selector: 'app-data-item',
  templateUrl: './data-item.component.html',
  styleUrls: ['./data-item.component.css']
})
export class DataItemComponent implements OnInit {
  @Input() file: SystemFile = {};
  @Output() onEditClick = new EventEmitter<number>();
  constructor() { }

  ngOnInit(): void {
  }

  onEdit() {
    this.onEditClick.emit(this.file.Id);
  }
}
