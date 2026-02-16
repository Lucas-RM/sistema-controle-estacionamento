import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VeiculoService } from '../../core/services/veiculo.service';
import { Veiculo, CreateVeiculoDto, UpdateVeiculoDto, TipoVeiculo } from '../../core/models/veiculo.model';
import { PagedResult } from '../../core/models/pagination.model';

@Component({
  selector: 'app-veiculos-list',
  templateUrl: './veiculos-list.component.html',
  styleUrls: ['./veiculos-list.component.css']
})
export class VeiculosListComponent implements OnInit {
  veiculos: Veiculo[] = [];
  totalRecords: number = 0;
  loading: boolean = false;
  
  mostrarModal: boolean = false;
  veiculoForm: FormGroup;
  editando: boolean = false;
  veiculoIdEdicao: string = '';
  
  tiposVeiculo = [
    { label: 'Carro', value: TipoVeiculo.Carro },
    { label: 'Moto', value: TipoVeiculo.Moto },
    { label: 'Caminhonete', value: TipoVeiculo.Caminhonete }
  ];
  
  filtroPlaca: string = '';

  constructor(
    private veiculoService: VeiculoService,
    private fb: FormBuilder
  ) {
    this.veiculoForm = this.fb.group({
      placa: ['', [Validators.required, Validators.pattern(/^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9][A-Z][0-9]{2}$|^[A-Z]{2}[0-9]{3}[A-Z]{2}$|^[A-Z]{3}[0-9]{3}$/)]],
      modelo: [''],
      cor: [''],
      tipo: [TipoVeiculo.Carro, Validators.required]
    });
  }

  ngOnInit() {
    this.carregarVeiculos();
  }

  carregarVeiculos() {
    this.loading = true;
    const params = this.filtroPlaca ? { placa: this.filtroPlaca } : {};
    
    this.veiculoService.listarVeiculos(params).subscribe(
      (result: PagedResult<Veiculo>) => {
        this.veiculos = result.data.map(v => {
          return ({ ...v })
        });
        this.totalRecords = result.pagination.totalItems;
        this.loading = false;
      },
      error => {
        console.error('Erro ao carregar veículos:', error);
        this.loading = false;
      }
    );
  }

  buscar() {
    this.carregarVeiculos();
  }

  abrirModalNovo() {
    this.editando = false;
    this.veiculoForm.reset({ tipo: TipoVeiculo.Carro });
    const placaControl = this.veiculoForm.get('placa');
    if (placaControl) {
      placaControl.enable();
    }
    this.mostrarModal = true;
  }

  abrirModalEditar(veiculo: Veiculo) {
    this.editando = true;
    this.veiculoIdEdicao = veiculo.id;
    // Normaliza tipo para número, mesmo se vier string
    let tipoNum: number;
    if (typeof veiculo.tipo === 'string') {
      // Tenta converter string para número
      tipoNum = parseInt(veiculo.tipo);
      if (isNaN(tipoNum)) {
        // Busca pelo label caso seja string tipo 'Carro', 'Moto', etc
        const tipoStr = veiculo.tipo as string;
        const tipoObj = this.tiposVeiculo.find(t => t.label.toLowerCase() === tipoStr.toLowerCase());
        tipoNum = tipoObj ? tipoObj.value : TipoVeiculo.Carro;
      }
    } else {
      tipoNum = veiculo.tipo;
    }

    this.veiculoForm.patchValue({
      placa: veiculo.placa,
      modelo: veiculo.modelo,
      cor: veiculo.cor,
      tipo: tipoNum
    });
    const placaControl = this.veiculoForm.get('placa');
    if (placaControl) {
      placaControl.disable();
    }
    this.mostrarModal = true;
  }

  salvar() {
    if (this.veiculoForm.invalid) return;
    
    this.loading = true;
    
    if (this.editando) {
      const dto: UpdateVeiculoDto = {
        modelo: this.veiculoForm.value.modelo,
        cor: this.veiculoForm.value.cor,
        tipo: this.veiculoForm.value.tipo
      };
      
      this.veiculoService.atualizarVeiculo(this.veiculoIdEdicao, dto).subscribe(
        () => {
          this.mostrarModal = false;
          this.carregarVeiculos();
          this.loading = false;
        },
        error => {
          console.error('Erro ao atualizar veículo:', error);
          alert(error.message);
          this.loading = false;
        }
      );
    } else {
      const dto: CreateVeiculoDto = {
        placa: this.veiculoForm.value.placa.toUpperCase().replace(/[\s-]/g, ''),
        modelo: this.veiculoForm.value.modelo,
        cor: this.veiculoForm.value.cor,
        tipo: this.veiculoForm.value.tipo
      };
      
      this.veiculoService.criarVeiculo(dto).subscribe(
        () => {
          this.mostrarModal = false;
          this.carregarVeiculos();
          this.loading = false;
        },
        error => {
          console.error('Erro ao criar veículo:', error);
          alert(error.message);
          this.loading = false;
        }
      );
    }
  }

  cancelar() {
    this.mostrarModal = false;
  }
}
