import { Component, OnInit, AfterViewInit, Input, ElementRef, EventEmitter, Output } from '@angular/core';
import Map from 'ol/Map';
import View from 'ol/View';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import XYZ from 'ol/source/XYZ';
import * as Proj from 'ol/proj';
import GPX from 'ol/format/GPX';
import VectorSource from 'ol/source/Vector';
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style';
import { defaults as defaultControls } from 'ol/control';

export const DEFAULT_HEIGHT = '500px';
export const DEFAULT_WIDTH = '500px';

export const DEFAULT_LAT = -34.603490361131385;
export const DEFAULT_LON = -58.382037891217465;

export const DEFAULT_ZOOM = 8;

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

  @Output() movestart = new EventEmitter<any>();
  @Output() moveend = new EventEmitter<any>();

  map!: Map;

  private mapEl!: HTMLElement;

  constructor(private elementRef: ElementRef) { }

  ngOnInit(): void {
    
    this.mapEl = this.elementRef.nativeElement.querySelector('#map');
    this.setSize();
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

    var vectorSource = new VectorSource({
      url: 'assets/fells_loop.gpx',
      format: new GPX(),
    });

    this.map = new Map({
      target: 'map',
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
    })


  }

  private setSize() {
    if (this.mapEl) {
      const styles = this.mapEl.style;
      styles.height = coerceCssPixelValue(this.height) || DEFAULT_HEIGHT;
      styles.width = coerceCssPixelValue(this.width) || DEFAULT_WIDTH;
    }
  }

}

const cssUnitsPattern = /([A-Za-z%]+)$/;

function coerceCssPixelValue(value: any): string {
  if (value == null) {
    return '';
  }
  return cssUnitsPattern.test(value) ? value : `${value}px`;
}
