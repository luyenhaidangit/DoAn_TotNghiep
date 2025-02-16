﻿using Electronic_WMS.Models.Models;
using Electronic_WMS.Repository.IRepository;
using Electronic_WMS.Repository.Repository;
using Electronic_WMS.Service.IService;
using Electronic_WMS.Utilities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Service.Service
{
    public class SerialNumberService : ISerialNumberService
    {
        private readonly ISerialNumberRepository _iSerialNumberRepository;
        private readonly IWareHouseRepository _iWareHouseRepository;
        public SerialNumberService(ISerialNumberRepository iSerialNumberRepository, IWareHouseRepository iWareHouseRepository)
        {
            _iSerialNumberRepository = iSerialNumberRepository;
            _iWareHouseRepository = iWareHouseRepository;
        }

        public SerialNumberVM GetById(int id)
        {
            var s = _iSerialNumberRepository.GetById(id);
            var SeriDetail = new SerialNumberVM
            {
                SerialId = s.SerialId,
                SerialNumber = s.SerialNumber,
                CreatedDate = s.CreatedDate,
                Location = s.Location,
                WareHouseId = s.WareHouseId,
                WareHouseName = _iWareHouseRepository.GetById(s.WareHouseId).Name,
            };
            return SeriDetail;
        }

        public GetListSerialByProductId GetListByProductId(SearchSeriVM search)
        {
            var listSeri = from s in _iSerialNumberRepository.GetListByProductId(search.ProductId)
                            select new SerialNumberVM
                            {
                                SerialId = s.SerialId,
                                SerialNumber = s.SerialNumber,
                                CreatedDate = s.CreatedDate,
                                Location = s.Location,
                                WareHouseId = s.WareHouseId,
                                WareHouseName = _iWareHouseRepository.GetById(s.WareHouseId).Name,
                            };
            if (!string.IsNullOrEmpty(search.TextSearch))
            {
                listSeri = listSeri.Where(s => s.SerialNumber.ToLower().Contains(search.TextSearch.ToLower()));
            }
            var total = listSeri.Count();
            listSeri = listSeri.Skip((search.CurrentPage - 1) * search.PageSize).Take(search.PageSize);
            return new GetListSerialByProductId { ListSerial = listSeri, Total = total };
        }

        public IEnumerable<ListSerialCombobox> GetListSerialCombobox(SearchListSerialCombobox search)
        {
            var listSeri = from s in _iSerialNumberRepository.GetList()
                           where s.WareHouseId == search.WareHouseId && s.ProductId == search.ProductId
                                 && (s.Status == (int)SeriStatus.IsStock || s.Status == (int)SeriStatus.IsProcessing)
                           select new ListSerialCombobox
                           {
                               SerialId = s.SerialId,
                               SerialNumber = s.SerialNumber
                           };
            return listSeri;
        }

        public ResponseModel UpdateLocation(List<UpdateLocation> listSeri)
        {
            if (listSeri.Count > 0)
            {
                List<string> lstLocation = new List<string>();
                foreach (var item in listSeri)
                {
                    // Check Location in database
                    var checkLocation = _iSerialNumberRepository.GetByLocationInWH(item.Location, item.WareHouseId);
                    if (checkLocation != null && checkLocation.SerialId != item.SerialId)
                    {
                        lstLocation.Add(checkLocation.Location);
                    }
                }
                if (lstLocation.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("These positions already exist in the warehouse: ");
                    foreach (var error in lstLocation)
                    {
                        errorMessage.AppendLine($"- {error}");
                    }
                    return new ResponseModel
                    {
                        StatusCode = 400,
                        StatusMessage = errorMessage.ToString()
                    };
                }
                foreach (var location in listSeri)
                {
                    var seri = _iSerialNumberRepository.GetById(location.SerialId);
                    if (seri == null)
                    {
                        return new ResponseModel
                        {
                            StatusCode = 404,
                            StatusMessage = "Serial does not exists!"
                        };
                    }

                    // Update  
                    seri.Location = location.Location;

                    var status = _iSerialNumberRepository.Update(seri);
                    if (status == 0)
                    {
                        return new ResponseModel
                        {
                            StatusCode = 500,
                            StatusMessage = "Error!"
                        };
                    }
                }
                return new ResponseModel
                {
                    StatusCode = 200,
                    StatusMessage = "Update Successfully!"
                };
            }
            else
            {
                return new ResponseModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error!"
                };
            }
        }
    }
}
