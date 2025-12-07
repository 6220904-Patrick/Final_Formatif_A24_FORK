import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
import { bounce, shake, tada } from 'ng-animate';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    animations: [
      trigger('shake', [transition(':increment', useAnimation(shake, {
        params: { timing: 2, delay: 0 }
      }))]),
      trigger('bounce', [transition(':increment', useAnimation(bounce, {
        params: { timing: 4, delay: 0 }
      }))]),
      trigger('tada', [transition(':increment', useAnimation(tada, {
        params: { timing: 3, delay: 0 }
      }))]),
    ]
})
export class AppComponent {
  title = 'ngAnimations';

  ng_shake = 0;
  ng_bounce = 0;
  ng_tada = 0;

  rotate = false;

  constructor() {
  }

  tourner() {
    if (this.rotate == false) {
      this.rotate = true;
      setTimeout(() => {
        this.rotate = false;
      }, 2000);
    }
  }

  animer(loop: boolean) {
    this.ng_shake++;
    setTimeout(() => {
      this.ng_bounce++;
      setTimeout(() => {
        this.ng_tada++;
        if (loop) {
          setTimeout(() => {
            this.animer(loop);
          }, 3000);
        }
      }, 3000);
    }, 2000);
  }
}
