import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RaceService } from '../_services/race.service';

@Component({
  selector: 'app-submit',
  templateUrl: './submit.component.html',
  styleUrls: ['./submit.component.css']
})
export class SubmitComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private raceService: RaceService) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      description: '',
      date: '',
      raceTypeId: '',
      totalLaps: '',
      file: ['']
    });
  }

  onFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.form.get('file').setValue(file);
    }
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('description', this.form.get('description').value);
    formData.append('date', this.form.get('date').value);
    formData.append('raceTypeId', this.form.get('raceTypeId').value);
    formData.append('totalLaps', this.form.get('totalLaps').value);
    formData.append('file', this.form.get('file').value);

    this.raceService.submit(formData).subscribe(
      (res) => {
        this.uploadResponse = res;
      },
      (err) => {
        this.error = err;
        //TODO - ignorar esse tratamento. Há um interceptor só pra isso.
      }
    );
  }

  form: FormGroup;
  error: string;
  uploadResponse: any = { status: '', message: '' };
}
