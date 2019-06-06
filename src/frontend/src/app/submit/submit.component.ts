import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { RaceService } from '../_services/race.service';
import { RaceType } from '../_models/racetype';
import { Router } from '@angular/router';
import { invalidDateValidatorFn } from '../_validators/invalid.date.validator';

@Component({
  selector: 'app-submit',
  templateUrl: './submit.component.html',
  styleUrls: ['./submit.component.css']
})
export class SubmitComponent implements OnInit {
  currentFormattedDate: string;
  raceTypes: RaceType[] = [];
  submitted: boolean = false;
  submitForm: FormGroup;
  error: string;
  uploadResponse: any = { status: '', response: '' };

  constructor(private router: Router, private formBuilder: FormBuilder, private raceService: RaceService) { }

  ngOnInit() {
    this.initializeCurrentDate();
    this.loadRaceTypes();

    this.submitForm = this.formBuilder.group({
      description: ['', Validators.required],
      date: ['', [Validators.required, invalidDateValidatorFn()]],
      raceTypeId: ['', Validators.required],
      totalLaps: ['', Validators.required],
      file: ['', Validators.required]
    });
  }

  initializeCurrentDate() {
    let currentDate = new Date();
    this.currentFormattedDate = `${currentDate.getFullYear()}-${currentDate.getMonth()}-${currentDate.getDay()}`;
  }

  get f() { return this.submitForm.controls; }

  onFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.submitForm.get('file').setValue(file);
    }
  }

  onSubmit() {
    this.submitted = true;

    if (this.submitForm.invalid) {
      return;
    }

    const formData = new FormData();
    formData.append('description', this.submitForm.get('description').value);
    formData.append('date', this.submitForm.get('date').value);
    formData.append('raceTypeId', this.submitForm.get('raceTypeId').value);
    formData.append('totalLaps', this.submitForm.get('totalLaps').value);
    formData.append('file', this.submitForm.get('file').value);

    this.raceService.submit(formData).subscribe(
      (res) => {
        this.uploadResponse = res;
        if (this.uploadResponse.status === 'finished') {
          this.router.navigate(['/race'], { queryParams: { raceId: this.uploadResponse.response.raceId } });
        }
      }
    );
  }

  loadRaceTypes() {
    this.raceService.listRaceTypes().subscribe(data => {
      this.raceTypes = data;
    });
  }
}
