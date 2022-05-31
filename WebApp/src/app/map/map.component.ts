import { Component, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import gMap from 'ol/Map';
import View from 'ol/View';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import XYZ from 'ol/source/XYZ';
import * as Proj from 'ol/proj';
import GPX from 'ol/format/GPX';
import VectorSource from 'ol/source/Vector';
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style';
import { defaults as defaultControls } from 'ol/control';
import { ApiService } from '../services/ApiService/api.service';

export const DEFAULT_HEIGHT = '500px';
export const DEFAULT_WIDTH = '500px';

export const DEFAULT_LAT = -34.603490361131385;
export const DEFAULT_LON = -58.382037891217465;

export const DEFAULT_ZOOM = 8;
export const DEFAULT_ID = '0';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, AfterViewInit {
  @Input() lat: number = DEFAULT_LAT;
  @Input() lon: number = DEFAULT_LON;
  @Input() zoom: number = DEFAULT_ZOOM;
  @Input() width: string | number = DEFAULT_WIDTH;
  @Input() height: string | number = DEFAULT_HEIGHT;
  @Input() idMap: string = DEFAULT_ID;

  @Output() movestart = new EventEmitter<any>();
  @Output() moveend = new EventEmitter<any>();

  map!: gMap;
  routeBlob: any;
  routeUrl: any;

  constructor(private service: ApiService) { }

  ngOnInit(): void {

  }

  ngAfterViewInit(): void {
    const style: any = {
      'Point': new Style({
        image: new CircleStyle({
          fill: new Fill({
            color: 'rgba(255,255,0,0.4)',
          }),
          radius: 5,
          stroke: new Stroke({
            color: '#ff0',
            width: 1,
          }),
        }),
      }),
      'LineString': new Style({
        stroke: new Stroke({
          color: '#f00',
          width: 3,
        }),
      }),
      'MultiLineString': new Style({
        stroke: new Stroke({
          color: '#0f0',
          width: 3,
        }),
      }),
    };

    const path: any = { path: 'Files\\Routes\\sfv\\Activities\\sfvRoute1.gpx' };
    this.service.getFriendRoute(path).subscribe({
      next: (resp) => {

        //let objectURL = URL.createObjectURL(this.routeBlob);
        let reader = new FileReader();
        reader.readAsDataURL(resp);

        reader.onload = _event => {
          this.routeUrl = reader.result;

          var vectorSource = new VectorSource({
            url: this.routeUrl,
            format: new GPX(),
          });

          this.map = new gMap({
            target: this.idMap,
            layers: [
              new TileLayer({
                source: new XYZ({
                  url: 'https://{a-c}.tile.openstreetmap.org/{z}/{x}/{y}.png'
                })
              }),
              new VectorLayer({
                source: vectorSource,

                style: (feature) => {
                  return style[feature.getGeometry()!.getType()];
                },
              })
            ],
            view: new View({
              center: Proj.fromLonLat([this.lon, this.lat]),
              zoom: this.zoom
            }),
            controls: defaultControls().extend([])
          });
        };



      },
      error: (error) => {
        console.log(error);
      }
    });

  }

  createRouteFromBlob(route: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.routeBlob.reader.result;
    }, false);

    if (route) {
      reader.readAsDataURL(route);
    }
  }

}

